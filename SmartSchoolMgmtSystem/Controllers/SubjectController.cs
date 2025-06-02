using SmartSchool.Utilities;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BAL;
using SmartSchool.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using SmartSchool.Models.Entity;

namespace SmartSchool.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ISubjectService _subjectService;
        private readonly MyDbContext _context;
        public SubjectController(ISubjectService subjectService,MyDbContext context)
        {
            _subjectService = subjectService;
            _context = context;
        }
        [Authorize(Policy = "School Admin")]
        public IActionResult GetSubject()
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
            var res = _subjectService.GetSubject(loggedInUser.userId);
            return View(res);
        }
        [HttpPost]
        public IActionResult AddSubject(SubjectDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _subjectService.AddSubject(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetSubject");
            }
        }
        [HttpPost]
        public IActionResult UpdateSubject(SubjectDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _subjectService.UpdateSubject(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetSubject");
            }
        }
        [HttpPost]
        public IActionResult DeleteSubject(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();
            response = _subjectService.DeleteSubject(id);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetSubject");
            }
        }
    }


}
