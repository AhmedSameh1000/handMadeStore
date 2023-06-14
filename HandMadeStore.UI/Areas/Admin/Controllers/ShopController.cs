using HandmadeStore.Models.Models.ViewModels;
using HandMadeStore.DataAccess.Data;
using HandMadeStore.DataAccess.Repository.IRepository;
using HandMadeStore.Models;
using HandMadeStore.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace HandMadeStore.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class ShopController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShopController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        ////GET Upsert Shop
        public IActionResult Upsert(int? id)
        {
            Shop Shop = new();

            if (id == null || id == 0)
            {
                //Create Shop
                //ViewBag.CategoryList = CategoryList;
                //ViewData["BrandList"] = BrandList;
                return View(Shop);
            }
            else
            {
                //Update Prodect
                Shop = _unitOfWork.Shop.GetFirstOrDefault(s => s.Id == id);
                return View(Shop);
            }
        }

        ////Post Upsert Shop

        [HttpPost]
        public IActionResult Upsert(Shop shop)
        {
            if (shop.Id == 0)
            {
                //Create new shop
                _unitOfWork.Shop.Add(shop);
                _unitOfWork.Save();
                TempData["success"] = "Shop created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                //Update shop
                _unitOfWork.Shop.Update(shop);
                _unitOfWork.Save();
                TempData["success"] = "Shop updated successfully";
                return RedirectToAction("Index");
            }
        }

        ////Delete Shop
        //GET
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var shop = _unitOfWork.Shop.GetFirstOrDefault(p => p.Id == id);
            if (shop == null)
            {
                return NotFound();
            }
            return View(shop);
        }

        //POSTS
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            var shop = _unitOfWork.Shop.GetFirstOrDefault(c => c.Id == id);
            if (shop is null)
            {
                return NotFound();
            }
            _unitOfWork.Shop.Remove(shop);
            _unitOfWork.Save();
            TempData.Add("success", "Shop deleted successfully");
            return RedirectToAction("Index");
        }

        #region APIENDPoints

        [HttpGet]
        public IActionResult GetAll()
        {
            var AllShops = _unitOfWork.Shop.GetAll();
            return Json(new { data = AllShops });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var shop = _unitOfWork.Shop.GetFirstOrDefault(p => p.Id == id);
            if (shop is null)
            {
                return Json(new { success = false, message = "Error while deleting shop" });
            }
            _unitOfWork.Shop.Remove(shop);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Shop deleted successfully" });
        }

        #endregion APIENDPoints
    }
}