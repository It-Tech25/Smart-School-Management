using SmartSchool.BAL;
using SmartSchool.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SmartSchool.Utilities;
using SmartSchool.Models.Entity;

namespace Advocate_Invoceing.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _service;
        private readonly MyDbContext _context;

        public HomeController(ILogger<HomeController> logger,IUserService service,MyDbContext context)
        {
            _logger = logger;
            _service = service;
            _context = context;
        }

        public IActionResult Index()
        {
            
            ViewBag.TotalSubscriptions = _service.TotalSubscriptions();

            return View();
        }
        public IActionResult AdminDashboard()
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            ViewBag.TotalSubscriptions = _service.TotalSubscriptions();
            ViewBag.SchoolName = _context.schools.Where(a => a.userid == loggedInUser.userId && a.IsDeleted == false).Select(a => a.Name).FirstOrDefault();
            int school = _context.schools.Where(a => a.userid == loggedInUser.userId && a.IsDeleted == false).Select(a => a.SchoolId).FirstOrDefault();
            ViewBag.pic1 = _context.schools.Where(a => a.userid == loggedInUser.userId && a.IsDeleted == false).Select(a => a.ProfilePhoto1).FirstOrDefault();
            ViewBag.pic2 = _context.schools.Where(a => a.userid == loggedInUser.userId && a.IsDeleted == false).Select(a => a.ProfilePhoto2).FirstOrDefault();
            ViewBag.pic3 = _context.schools.Where(a => a.userid == loggedInUser.userId && a.IsDeleted == false).Select(a => a.ProfilePhoto3).FirstOrDefault();
            int sid = _context.userTypeEntites.Where(a => a.UserTypeName == "Student" && a.CreatedBy == loggedInUser.userId && a.IsDeleted == false).Select(a => a.UserTypeId).FirstOrDefault();
            int tid = _context.userTypeEntites.Where(a => a.UserTypeName == "Teacher" && a.CreatedBy == loggedInUser.userId && a.IsDeleted == false).Select(a => a.UserTypeId).FirstOrDefault();
            ViewBag.TotalTeacher = _context.userEntity.Where(a => a.UserTypeId == tid && a.CreatedBy == loggedInUser.userId && a.IsDeleted == false).Count();
            ViewBag.TotalStudent = _context.studentEntity.Where(a => a.UserTypeId == sid && a.CreatedBy == loggedInUser.userId && a.IsDeleted == false).Count();
            ViewBag.TotalFee = _context.studentEntity.Where(f => f.IsDeleted == false && f.SchoolId==school).Sum(f => f.TotalFee);
            ViewBag.CollectedFee = _context.feePaymentEntity.Where(f => f.IsDeleted == false && f.CreatedBy == loggedInUser.userId).Sum(f => f.Amount);
            ViewBag.Name = _context.userEntity.Where(a => a.UserId == loggedInUser.userId && a.IsDeleted == false).Select(a => a.FullName).FirstOrDefault();

            ViewBag.Profile = _context.userEntity.Where(a => a.UserId == loggedInUser.userId && a.IsDeleted == false).Select(a => a.ProfilePicture).FirstOrDefault();
            ViewBag.Name = _context.userEntity.Where(a => a.UserId == loggedInUser.userId && a.IsDeleted == false).Select(a => a.FullName).FirstOrDefault();

            return View();
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



    }
}
