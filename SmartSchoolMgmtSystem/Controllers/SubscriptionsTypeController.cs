using SmartSchool.Utilities;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BAL;
using SmartSchool.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace SmartSchool.Controllers
{
    public class SubscriptionsTypeController : Controller
    {
        private readonly ISubscriptionsTypeServices _SubscriptionsTypeServices;
        public SubscriptionsTypeController(ISubscriptionsTypeServices SubscriptionsTypeServices)
        {
            _SubscriptionsTypeServices = SubscriptionsTypeServices;

        }

        [Authorize(Policy = "Super Admin")]
        public IActionResult GetSubscriptionsType(string? state)
        {
            var states = _SubscriptionsTypeServices.GetStates();
            ViewBag.State = states;
            var cities = _SubscriptionsTypeServices.Getcity(state);
            ViewBag.city = cities;
            var res = _SubscriptionsTypeServices.GetAllSubscriptionsType();
            return View(res);
        }

        // Add a new School address
        [HttpPost]
        public IActionResult AddSubscriptionsType(SubscriptionsTypeDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            GenericResponse response = _SubscriptionsTypeServices.AddSub(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetSubscriptionsType");
            }
        }

        // Update School address
        [HttpPost]
        public IActionResult UpdateSubscriptionsType(SubscriptionsTypeDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            GenericResponse response = _SubscriptionsTypeServices.UpdateSubscriptionsType(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetSubscriptionsType");
            }
        }

        // Delete School address
        [HttpPost]
        public IActionResult DeleteSubscriptionsType(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            GenericResponse response = _SubscriptionsTypeServices.DeleteSubscriptionsType(id);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetSubscriptionsType");
            }
        }

        [HttpGet]
        public JsonResult GetCitiesByState(string state)
        {
            var cities = _SubscriptionsTypeServices.Getcity(state);

            return Json(cities);
        }
    }
}

