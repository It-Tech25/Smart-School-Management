using SmartSchool.Models.Entity;
using SmartSchool.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.BAL;
using SmartSchool.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace SmartSchool.Controllers
{
    public class FeePaymentsController : Controller
    {
        private readonly IFeePaymentsService _feePaymentsService;
        private readonly MyDbContext _context;
        public FeePaymentsController(IFeePaymentsService feePaymentsService, MyDbContext context)
        {
            _feePaymentsService = feePaymentsService;
            _context = context;
        }
        [Authorize(Policy = "School Admin")]
        public IActionResult GetFeePayments(int? studentId)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.Students = _context.studentEntity
           .Where(s => s.IsDeleted == false)
           .Select(s => new StudentDTO
            {
             StudentId = s.StudentId,
             // Name = s.Name  // or just s.Name if available
            }).ToList();

            ViewBag.FeeItems = _context.feeItemEntity
                .Where(f => f.IsDeleted == false)
                .Select(f => new FeeItemsDto
                {
                    ItemId = f.ItemId,
                  //  ItemName = f.ItemName // or appropriate property name
                }).ToList();


            var feePayments = _context.feePaymentEntity
              .AsQueryable()
              .Select(fp => new FeePaymentsDTO
              {
                  PaymentId = fp.PaymentId,
                  StudentId = fp.StudentId,
                  Name = _context.studentEntity.Where(a => a.StudentId == fp.StudentId).Select(a => a.StudentName).FirstOrDefault(),
                  ItemId = fp.ItemId,
                  PaymentDate = fp.PaymentDate,
                  Amount = fp.Amount,
                  ReceiptNumber = fp.ReceiptNumber
              });


            if (studentId.HasValue)
            {
                feePayments = feePayments.Where(f => f.StudentId == studentId.Value);
                return View(feePayments);
            }
            else
            {
                var res = _feePaymentsService.GetFeePayments(loggedInUser.userId);
                return View(res);
            }
         
        
        }
        [HttpPost]
        public IActionResult AddFeePayments(FeePaymentsDTO obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _feePaymentsService.UpdateFeePayments(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetFeePayments");
            }
        }
        [HttpPost]
        public IActionResult UpdateFeePayments(FeePaymentsDTO obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _feePaymentsService.UpdateFeePayments(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetFeePayments");
            }
        }
        [HttpPost]
        public IActionResult DeleteFeePayments(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();
            response = _feePaymentsService.DeleteFeePayments(id);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetFeePayments");
            }
        }
    }
}
