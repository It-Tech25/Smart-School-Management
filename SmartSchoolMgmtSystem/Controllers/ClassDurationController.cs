using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BAL;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;
using SmartSchool.Utilities;

namespace SmartSchool.Controllers
{
    public class ClassDurationController : Controller
    {
        private readonly IClassDurationService _classService;
        private readonly MyDbContext _context;
        public ClassDurationController(IClassDurationService classService,MyDbContext context)
        {
            _classService = classService;
            _context = context;
        }
        [Authorize(Policy = "School Admin")]
        public IActionResult Classtiming()
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
            var res = _classService.Classtiming(loggedInUser.userId);
            return View(res);
        }
        [HttpPost]
        public IActionResult AddClasstiming(ClassDurationDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _classService.AddClasstiming(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("Classtiming");
            }
        }
        [HttpPost]
        public IActionResult UpdateClasstiming(ClassDurationDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _classService.UpdateClasstiming(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("Classtiming");
            }
        }
        [HttpPost]
        public IActionResult DeleteClassDuration(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();
            response = _classService.DeleteClassDuration(id);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("Classtiming");
            }
        }
    }

}
