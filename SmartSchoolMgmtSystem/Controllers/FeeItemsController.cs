using SmartSchool.Models.Entity;
using SmartSchool.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartSchool.BAL;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;
using Microsoft.AspNetCore.Authorization;

namespace SmartSchool.Controllers
{
    public class FeeItemsController : Controller
    {
        private readonly IFeeItemsService _feeItemsService;
        private readonly IFeeCategoriesService _feeCategories;
        private readonly MyDbContext _context;
        public FeeItemsController(IFeeItemsService feeItemsService,IFeeCategoriesService feeCategoriesService,MyDbContext context)
        {
            _feeItemsService = feeItemsService;
            _feeCategories = feeCategoriesService;
           
        }
        [Authorize(Policy = "School Admin")]
        public IActionResult GetFeeItems()
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            var catagories = _feeCategories.GetFeeCategoryName(loggedInUser.userId);
            ViewBag.Categories = catagories;
           var res = _feeItemsService.GetFeeItems(loggedInUser.userId);
            return View(res);
            
        }
        [HttpPost]
        public IActionResult AddFeeItems(FeeItemsDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _feeItemsService.AddFeeItems(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetFeeItems");
            }
        }
        [HttpPost]
        public IActionResult UpdateFeeItems(FeeItemsDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _feeItemsService.UpdateFeeItems(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetFeeItems");
            }
        }
        [HttpPost]
        public IActionResult DeleteFeeItems(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();
            response = _feeItemsService.DeleteFeeItems(id);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetFeeItems");
            }
        }
    }
}
