using SmartSchool.Utilities;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BAL;
using SmartSchool.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace SmartSchool.Controllers
{
    public class FeeCategoryController : Controller
    {
        private readonly IFeeCategoriesService _feeCategoriesService;
        public FeeCategoryController(IFeeCategoriesService feeCategoriesService)
        {
            _feeCategoriesService = feeCategoriesService;
        }
        [Authorize(Policy = "School Admin")]
        public IActionResult GetFeeCategoryName()
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            var res = _feeCategoriesService.GetFeeCategoryName(loggedInUser.userId);
            return View(res);
        }
        [HttpPost]
        public IActionResult AddFeeCategory(FeeCatagoriesDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _feeCategoriesService.AddFeeCategory(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetFeeCategoryName");
            }
        }
        [HttpPost]
        public IActionResult UpdateFeeCategory(FeeCatagoriesDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _feeCategoriesService.UpdateFeeCategory(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetFeeCategoryName");
            }
        }
        [HttpPost]
        public IActionResult DeleteFeeCategory(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();
            response = _feeCategoriesService.DeleteFeeCategory(id);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetFeeCategoryName");
            }
        }

    }
}
