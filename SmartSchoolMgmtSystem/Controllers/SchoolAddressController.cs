using SmartSchool.Models.Entity;
using SmartSchool.Utilities;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BAL;
using SmartSchool.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace SmartSchool.Controllers
{
    public class SchoolAddressController : Controller
    {
        private readonly ISchoolAddressServices _schoolAddressService;
        public SchoolAddressController(ISchoolAddressServices schoolAddressService)
        {
            _schoolAddressService = schoolAddressService;

        }

        [Authorize(Policy = "SuperAdmin")]
        public IActionResult GetSchoolAddresses(string? state)
        {
            var states = _schoolAddressService.GetStates();
            ViewBag.State = states;
            var cities = _schoolAddressService.Getcity(state);
            ViewBag.city = cities;
            var res = _schoolAddressService.GetAllSchoolAddresses();
            return View(res);
        }

        // Add a new school address
        [HttpPost]
        public IActionResult AddSchoolAddress(SchoolAddressDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            GenericResponse response = _schoolAddressService.AddSchoolAddress(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetSchoolAddresses");
            }
        }

        // Update school address
        [HttpPost]
        public IActionResult UpdateSchoolAddress(SchoolAddressDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            GenericResponse response = _schoolAddressService.UpdateSchoolAddress(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetSchoolAddresses");
            }
        }

        // Delete school address
        [HttpPost]
        public IActionResult DeleteSchoolAddress(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            GenericResponse response = _schoolAddressService.DeleteSchoolAddress(id);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetSchoolAddresses");
            }
        }

        [HttpGet]
        public JsonResult GetCitiesByState(string state)
        {
            var cities = _schoolAddressService.Getcity(state);

            return Json(cities);
        }
    }

}

