using HandMadeStore.DataAccess.Data;
using HandMadeStore.DataAccess.Repository.IRepository;
using HandMadeStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HandMadeStore.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BrandController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Brand> Brands = _unitOfWork.Brand.GetAll();
            return View(Brands);
        }

        ////Create Brand
        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            if (!string.IsNullOrEmpty(brand.Name))
            {
                var duplicatedBrand = _unitOfWork.Brand
                    .GetFirstOrDefault(p => p.Name.ToLower() == brand.Name.ToLower());
                if (duplicatedBrand != null)
                {
                    //ModelState.AddModelError(String.Empty, "This brand name is duplicated.");
                    ModelState.AddModelError("name", "This brand name is duplicated.");
                }
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Brand.Add(brand);
                _unitOfWork.Save();
                TempData.Add("success", "Brand created successfully");
                return RedirectToAction("Index");
            }
            return View(brand);
        }

        ////Update Brand
        //GET
        public IActionResult Update(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var brand = _unitOfWork.Brand.GetFirstOrDefault(p => p.Id == id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        //POST
        [HttpPost]
        public IActionResult Update(Brand brand)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Brand.Update(brand);
                _unitOfWork.Save();
                TempData.Add("success", "Brand updated successfully");
                return RedirectToAction("Index");
            }
            return View(brand);
        }

        ////Delete Brand
        //GET
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var brand = _unitOfWork.Brand.GetFirstOrDefault(p => p.Id == id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        //POSTS
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            var brand = _unitOfWork.Brand.GetFirstOrDefault(c => c.Id == id);
            if (brand is null)
            {
                return NotFound();
            }
            _unitOfWork.Brand.Remove(brand);
            _unitOfWork.Save();
            TempData.Add("success", "Brand deleted successfully");
            return RedirectToAction("Index");
        }
    }
}