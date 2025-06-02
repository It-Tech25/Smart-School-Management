using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.BAL;
using SmartSchool.BAL;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;
using SmartSchool.Utilities;

namespace SmartSchool.Controllers
{
    public class ClassController : Controller
    {
        private readonly IClassesService _class;
        private readonly MyDbContext _context;
        public ClassController(IClassesService classes,MyDbContext context)
        {
            _class = classes;
            _context = context;
        }
        [Authorize(Policy = "School Admin")]
        public IActionResult GetClasses()
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

            var res = _class.GetClass(loggedInUser.userId);
            return View(res);
        }
        [HttpPost]
        public IActionResult AddClass(ClassesDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _class.AddClass(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public IActionResult UpdateClass(ClassesDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _class.UpdateClass(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false,response.message });
            }
        }
        [HttpPost]
        public IActionResult DeleteClass(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();
            response = _class.DeleteClass(id);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

    }
}
