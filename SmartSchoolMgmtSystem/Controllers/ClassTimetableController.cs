using SmartSchool.Models.Entity;
using SmartSchool.Utilities;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BAL;
using SmartSchool.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace SmartSchool.Controllers
{
    public class ClassTimetableController : Controller
    {
        private readonly IClassTimetableServices _ClassTimetableServices;
        private readonly ISubjectService _Subject;
        private readonly IClassesService _classService;
        private readonly MyDbContext _context;
        public ClassTimetableController(IClassTimetableServices ClassTimetableServices,ISubjectService subject, IClassesService classService,MyDbContext context)
        {
            _ClassTimetableServices = ClassTimetableServices;
            _Subject = subject;
            _classService = classService;
            _context = context;
        }

        [Authorize(Policy = "School Admin")]
        public IActionResult GetClassTimetable(string? state)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            var states = _ClassTimetableServices.GetStates();
            ViewBag.State = states;
            var cities = _ClassTimetableServices.Getcity(state);
            ViewBag.city = cities;
            var res = _ClassTimetableServices.GetAllClassTimetable();
            ViewBag.Classes = _classService.GetClass(loggedInUser.userId);
            int id = _context.userTypeEntites.Where(a => a.CreatedBy == loggedInUser.userId && a.UserTypeName == "Teacher").Select(a => a.UserTypeId).FirstOrDefault();
           var respon = _context.userEntity.Where(u => u.IsDeleted == false && u.UserTypeId == id && u.CreatedBy == loggedInUser.userId).ToList();
            ViewBag.Teachers = respon.ToString();
            var teacher = _context.userEntity.Where(a => a.UserTypeId == id && a.IsDeleted == false && a.CreatedBy == loggedInUser.userId).ToList();
            ViewBag.Teacher = teacher;
            ViewBag.Subjects = _Subject.GetSubject(loggedInUser.userId);
            return View(res);
        }
        // Add a new ClassTimetable
        [HttpPost]
        public IActionResult AddClassTimetable(ClassTimetableDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            GenericResponse response = _ClassTimetableServices.AddClassTimetable(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetClassTimetable");
            }
        }

        // Update ClassTimetable
        [HttpPost]
        public IActionResult UpdateClassTimetable(ClassTimetableDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            GenericResponse response = _ClassTimetableServices.UpdateClassTimetable(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetClassTimetable");
            }
        }

        // Delete ClassTimetable
        [HttpPost]
        public IActionResult DeleteClassTimetables(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            GenericResponse response = _ClassTimetableServices.DeleteClassTimetable(id);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetClassTimetable");
            }
        }

        [HttpGet]
        public JsonResult GetCitiesByState(string state)
        {
            var cities = _ClassTimetableServices.Getcity(state);

            return Json(cities);
        }
    }
}
