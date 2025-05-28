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
        public IActionResult Index(int? schoolid, string status, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.subscriptionsPaymentEntity.AsQueryable();

            if (schoolid.HasValue)
                query = query.Where(p => p.schoolid == schoolid);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(p => p.Status == status);

            if (startDate.HasValue && endDate.HasValue)
                query = query.Where(p => p.PaidDate >= startDate && p.PaidDate <= endDate);

            var payments = query
                .Where(p => p.IsDeleted == false || p.IsDeleted == null)
                .Select(p => new SubscriptionPaymentsEntity
                {
                    PaymentId = p.PaymentId,
                    SubscriptionId = p.SubscriptionId,
                    Amount = p.Amount,
                    PaidDate = p.PaidDate,
                    Status = p.Status,
                    Modules = p.Modules,
                    CreatedOn = p.CreatedOn
                    // Add others as needed
                }).ToList();

            return View(payments);
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
