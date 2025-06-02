using SmartSchool.Utilities;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BAL;
using SmartSchool.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using SmartSchool.Models.Entity;

namespace SmartSchool.Controllers
{
    public class FeeCategoryController : Controller
    {
        private readonly IFeeCategoriesService _feeCategoriesService;
        private readonly MyDbContext _context;

        public FeeCategoryController(IFeeCategoriesService feeCategoriesService,MyDbContext context)
        {
            _feeCategoriesService = feeCategoriesService;
            _context = context;
        }
        [Authorize(Policy = "School Admin")]
        public IActionResult GetFeeCategoryName()
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            var school = _context.schools
    .Where(a => a.userid == loggedInUser.userId && a.IsDeleted == false)
    .FirstOrDefault();
            ViewBag.SchoolLogo = school.Logo;
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
