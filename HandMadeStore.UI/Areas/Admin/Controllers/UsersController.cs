using HandMadeStore.DataAccess.Repository.IRepository;
using Identity.Models;
using Identity.Models.AllRolesViewModel;
using Identity.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace HandMadeStore.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;

        public UsersController(UserManager<ApplicationUser> userManager
        , RoleManager<IdentityRole> roleManager,
            IUserStore<ApplicationUser> userStore, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _unitOfWork = unitOfWork;
            _emailStore = GetEmailStore();
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Select(user => new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                Roles = _userManager.GetRolesAsync(user).Result
            }).Where(c => c.Email != "Admin@gmail.com").ToListAsync();

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Mange(string id)
        {
            var UserSelected = await _userManager.FindByIdAsync(id);
            if (UserSelected is null) return NotFound();

            var Roles = await _roleManager.Roles.ToListAsync();

            var ViewModel = new UserRolesViewModel()
            {
                UserId = UserSelected.Id,
                UserName = UserSelected.UserName,
                Roles = Roles.Select(Role => new RolesViewModels()
                {
                    RoleId = Role.Id,
                    RoleName = Role.Name,
                    IsSelected = _userManager.IsInRoleAsync(UserSelected, Role.Name).Result
                }).ToList()
            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Mange(UserRolesViewModel model)
        {
            var UserSelected = await _userManager.FindByIdAsync(model.UserId);
            if (UserSelected is null) return NotFound();

            var UserRoles = await _userManager.GetRolesAsync(UserSelected);
            foreach (var Role in model.Roles)
            {
                if (UserRoles.Any(r => r == Role.RoleName) && !Role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(UserSelected, Role.RoleName);

                if (!UserRoles.Any(r => r == Role.RoleName) && Role.IsSelected)
                    await _userManager.AddToRoleAsync(UserSelected, Role.RoleName);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Add()
        {
            var Roles = await _roleManager.Roles.Select(r => new RolesViewModels()
            {
                RoleId = r.Id,
                RoleName = r.Name,
            }).ToListAsync();

            var ViewModel = new AddUserViewModel()
            {
                Roles = Roles,
            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            //if (!model.Roles.Any(r => r.IsSelected))
            //{
            //    ModelState.AddModelError("Roles", "Select At least One Role");
            //    return View(model);
            //}
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "Email is Already Used");
                return View(model);
            }

            var user = new ApplicationUser()
            {
                Name = model.Name,
                Email = model.Email,
                City = model.City,
                PostalCode = model.PostalCode,
                StreetAddress = model.StreetAddress,
            };
            await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Roles", error.Description);
                }
                return View(model);
            }
            await _userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var UserSelected = await _userManager.FindByIdAsync(id);
            if (UserSelected is null) return NotFound();

            var ViewModel = new ProfileFormViewModel()
            {
                Id = id,
                name = UserSelected.Name,
                Email = UserSelected.Email
            };
            return View(ViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProfileFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var UserSelected = await _userManager.FindByIdAsync(model.Id);
            if (UserSelected is null) return NotFound();
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userWithSameEmail != null && userWithSameEmail.Id != model.Id)
            {
                ModelState.AddModelError("Email", "Email is Already Used");
                return View(model);
            }
            UserSelected.Name = model.name;

            UserSelected.Email = model.Email;

            await _userManager.UpdateAsync(UserSelected);
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var UserSelected = await _userManager.FindByIdAsync(id);
            if (UserSelected is null) return NotFound("this User not found");
            var relatedDataOnOrdrHeader = _unitOfWork.OrderHeader.GetAll(c => c.ApplicationUserId == id);
            _unitOfWork.OrderHeader.RemoveRange(relatedDataOnOrdrHeader);
            _unitOfWork.Save();
            var Result = await _userManager.DeleteAsync(UserSelected);
            if (!Result.Succeeded)
            {
                throw new Exception();
            }
            return Ok("User Deleted Successfuly");
        }
    }
}