
using SmartSchool.Models.Entity;
using SmartSchool.Utilities;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BAL;
using SmartSchool.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace SmartSchool.Controllers
{
    public class DurationController : Controller
    {
        private readonly IUserService _user;
        private readonly MyDbContext _context;
        private readonly ISchoolAddressServices _schoolAddressService;
        public DurationController(MyDbContext context, IUserService user, ISchoolAddressServices schoolAddressServices)
        {
            _user = user;
            _context = context;
            _schoolAddressService = schoolAddressServices;
        }
        [Authorize(Policy = "Super Admin")]
        public IActionResult GetDuration()
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            var res = _user.GetDuration();
            return View(res);
        }
        [HttpPost]
        public IActionResult AddDuration(DurationDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _user.AddDuration(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetDuration");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateDuration(DurationDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _user.UpdateDuration(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetDuration");
            }
        }
        [HttpPost]
        public IActionResult DeleteDuration(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();
            response = _user.DeleteDuration(id);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetDuration");
            }
        }

    }
}
