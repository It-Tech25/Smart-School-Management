using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.BAL;
using SmartSchool.Models;
using SmartSchool.Models.Entity;
using SmartSchool.Service;
using SmartSchool.Utilities;
using System.Diagnostics;

namespace Advocate_Invoceing.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _service;
        private readonly ITeachersServices _tservice;
        private readonly IStudentService _sservice;
        private readonly IFeePaymentsService _fservice;
        private readonly MyDbContext _context;

        public HomeController(ILogger<HomeController> logger,IUserService service,MyDbContext context, ITeachersServices tservice,IStudentService sservice,IFeePaymentsService fservice)
        {
            _logger = logger;
            _service = service;
            _context = context;
            _tservice = tservice;
            _sservice = sservice;
            _fservice = fservice;
        }

        public IActionResult Index()
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.Profile = _context.userEntity.Where(a => a.UserId == loggedInUser.userId && a.IsDeleted == false).Select(a => a.ProfilePicture).FirstOrDefault();
            ViewBag.Name = _context.userEntity.Where(a => a.UserId == loggedInUser.userId && a.IsDeleted == false).Select(a => a.FullName).FirstOrDefault();
            ViewBag.SchoolLogo = loggedInUser.schoolLogo;
            ViewBag.TotalSubscriptions = _service.TotalSubscriptions();

            return View();
        }
        public IActionResult AdminDashboard(string schoolName)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            var school = _context.schools
                .Where(a => a.userid == loggedInUser.userId && a.IsDeleted == false)
                .FirstOrDefault();

            if (school == null || !string.Equals(school.Name, schoolName, StringComparison.OrdinalIgnoreCase))
            {
                // Either school mismatch or invalid school — redirect to correct one
                return RedirectToAction("AdminDashboard", "Home", new { schoolName = school?.Name ?? "Unknown" });
            }

            ViewBag.SchoolName = school.Name;
            ViewBag.pic1 = school.ProfilePhoto1;
            ViewBag.pic2 = school.ProfilePhoto2;
            ViewBag.pic3 = school.ProfilePhoto3;
            ViewBag.SchoolLogo = school.Logo;

            int sid = _context.userTypeEntites
                .Where(a => a.UserTypeName == "Student" && a.CreatedBy == loggedInUser.userId && a.IsDeleted==false)
                .Select(a => a.UserTypeId).FirstOrDefault();

            int tid = _context.userTypeEntites
                .Where(a => a.UserTypeName == "Teacher" && a.CreatedBy == loggedInUser.userId && a.IsDeleted == false)
                .Select(a => a.UserTypeId).FirstOrDefault();

            ViewBag.TotalTeacher = _context.userEntity
                .Count(a => a.UserTypeId == tid && a.CreatedBy == loggedInUser.userId && a.IsDeleted == false);

            ViewBag.TotalStudent = _context.studentEntity
                .Count(a => a.UserTypeId == sid && a.CreatedBy == loggedInUser.userId && a.IsDeleted == false);

            ViewBag.TotalFee = _context.studentEntity
                .Where(f => f.IsDeleted == false && f.SchoolId == school.SchoolId)
                .Sum(f => f.TotalFee);

            ViewBag.CollectedFee = _context.feePaymentEntity
                .Where(f => f.IsDeleted == false && f.CreatedBy == loggedInUser.userId)
                .Sum(f => f.Amount);

            ViewBag.Name = _context.userEntity
                .Where(a => a.UserId == loggedInUser.userId && a.IsDeleted == false)
                .Select(a => a.FullName).FirstOrDefault();

            ViewBag.Profile = _context.userEntity
                .Where(a => a.UserId == loggedInUser.userId && a.IsDeleted == false)
                .Select(a => a.ProfilePicture).FirstOrDefault();

            return View();
        }
        public IActionResult GetSubscription()
        {
            var query = _context.subscriptionsPaymentEntity.AsQueryable();

           
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
        public IActionResult GetTeachers()
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            var teachers = _tservice.GetAllAsync(loggedInUser.userId);

            return Json(teachers);
        }
        public IActionResult GetStudents()
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            var students = _sservice.GetStudent(loggedInUser.userId);

            return Json(students);
        }
        public IActionResult GetFeePayments()
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            var feePayments = _fservice.GetFeePayments(loggedInUser.userId);

            return Json(feePayments);
        }
        public IActionResult TeacherDashboard()
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            ViewBag.TotalSubscriptions = _service.TotalSubscriptions();
            ViewBag.Name = _context.userEntity.Where(a => a.UserId == loggedInUser.userId && a.IsDeleted == false).Select(a => a.FullName).FirstOrDefault();
            ViewBag.Studentlist = GetStudentpresence();
            ViewBag.SchoolLogo = loggedInUser.schoolLogo;
            ViewBag.Profile = _context.userEntity.Where(a => a.UserId == loggedInUser.userId && a.IsDeleted == false).Select(a => a.ProfilePicture).FirstOrDefault();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

		public IActionResult TableCopy()
		{
			return View();
		}


        [HttpGet]
        public IActionResult GetSubscriptions()
        {
            try
            {
                var subscriptions = _service.GetSubscriptions(); // Call your service here
                return Json(new { success = true, data = subscriptions });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
		public IActionResult GetStudentpresence()
		{
			var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");

			if (loggedInUser == null)
			{
				return RedirectToAction("Login", "Authenticate");
			}
			var students = _sservice.GetStudent(loggedInUser.userId);
            
			return View(students);
		}



	}
}
