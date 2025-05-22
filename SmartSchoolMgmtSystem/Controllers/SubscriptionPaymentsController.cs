using SmartSchool.Models.Entity;
using SmartSchool.Utilities;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BAL;
using SmartSchool.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace SmartSchool.Controllers
{
    public class SubscriptionPaymentsController : Controller
    {
        private readonly ISubscriptionPaymentsService _SubscriptionPaymentsService;
        private readonly MyDbContext _context;
        public SubscriptionPaymentsController(ISubscriptionPaymentsService subscriptionPaymentsService,MyDbContext context)
        {
            _SubscriptionPaymentsService = subscriptionPaymentsService;
            _context = context;
        }
        [Authorize(Policy = "Super Admin")]
        public IActionResult GetPayment()
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            var stype = _context.subscriptionsTypeEntity.Where(a => a.IsDeleted == false).ToList();
            ViewBag.SType = stype;
            ViewBag.Schools = _context.schools.Where(a=>a.IsDeleted==false).Select(x =>x.Name).ToList();
            ViewBag.Duration = _context.duration.Where(a => a.IsDeleted == false).Select(x =>x.DurationType ).ToList();
            ViewBag.Modules = _context.module.Where(a => a.IsDeleted == false).Select(x=>x.Name ).ToList();

            var res = _SubscriptionPaymentsService.GetPaymentDetails();
            return View(res);
        }
        [HttpPost]
        public IActionResult AddPayment(SubscriptionPaymentsDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _SubscriptionPaymentsService.AddPayment(obj, loggedInUser.userId);
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
        public IActionResult UpdatePayment(SubscriptionPaymentsDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _SubscriptionPaymentsService.UpdatePayment(obj, loggedInUser.userId);
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
        public IActionResult DeletePayment(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();
            response = _SubscriptionPaymentsService.DeletePayment(id);
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
