using Microsoft.AspNetCore.Mvc;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;
using SmartSchool.Utilities;

namespace SmartSchool.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _config;
        public AccountController(MyDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetObjectAsJson("loggedUser", null);
            return RedirectToAction("Login", "Authenticate");
        }
        private static Profile _mockUser = new Profile
        {
            Id = 1,
            FullName = "John Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        [HttpGet]
        public IActionResult Profile()
        {
            return View(_mockUser);
        }

        [HttpPost]
        public IActionResult Profile(Profile model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Save the model to database — here we just update the dummy object
            _mockUser = model;

            ViewBag.Message = "Profile updated successfully!";
            return View(_mockUser);
        }

    }
}
