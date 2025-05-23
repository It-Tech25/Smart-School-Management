using SmartSchool.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SmartSchool.BAL;
using SmartSchool.Models.Entity;
using SmartSchool.Models.DTO;
using System.Net.Mail;

namespace Advocate_Invoceing.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly IUserService _Service;
        private readonly MyDbContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;

        public AuthenticateController(IUserService Service, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, MyDbContext context, IConfiguration config)
        {
            _Service = Service;
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _config = config;
        }

        public IActionResult Login(string schoolName, string role)
        {
            // Handle logo and user info from TempData
            ViewBag.SchoolLogo = TempData["SchoolLogo"] ?? "/uploads/default-logo.png";
            ViewBag.UserType = TempData["UserType"];
            ViewBag.SchoolName = schoolName;
            ViewBag.Role = role;

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = _Service.LoginCheck(request);

                if (result.statusCode == 0)
                {
                    ViewBag.ErrorMessage = result.Message;
                    return View(request);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, result.userId.ToString()),
                    new Claim(ClaimTypes.Name, result.userName),
                    new Claim("UserType", result.userTypeName)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                              new ClaimsPrincipal(claimsIdentity), authProperties);

                HttpContext.Session.SetString("UserId", result.userId.ToString());
                HttpContext.Session.SetString("UserName", result.userName);
                HttpContext.Session.SetObjectAsJson("loggedUser", result);

                return result.userTypeName switch
                {
                    "Super Admin" => RedirectToAction("Index", "Home"),
                    "School Admin" => RedirectToAction("AdminDashboard", "Home"),
                    "Teacher" => RedirectToAction("TeacherDashboard", "Home"),
                    _ => RedirectToAction("Login") // fallback
                };
            }

            return View(request);
        }

        public async Task<IActionResult> ForgotPassword()
        {
            UserDto r = new UserDto();
            return View(r);
        }
        //[Route("guardians/{schoolName}/{role}")]
        //public IActionResult Login(string schoolName, string role)
        //{
        //    // Pass these to the view to dynamically set logo and other info
        //    ViewBag.SchoolName = schoolName;
        //    ViewBag.Role = role;

        //    return View(new LoginRequest());
        //}
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(UserDto request)
        {
            var user = await _Service.GetUserByEmailAsync(request.Email);
            if (user == null || user.IsDeleted == true)
            {
                ModelState.AddModelError(string.Empty, "The email address is not associated with an active account.");
                return View(request);
            }

            string generatedOtp = GenerateRandomOTP();
            HttpContext.Session.SetString("GeneratedOtp", generatedOtp);
            HttpContext.Session.SetString("Email", request.Email);
            SendEmail(request.Email, generatedOtp);
            ViewBag.ShowOtpModal = true;

            return View("VerifyOtp", request);
        }

        public async Task<IActionResult> ResetPassword()
        {
            return View(new ResetPasswordFinal());
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordFinal request)
        {
            string generatedOtp = HttpContext.Session.GetString("GeneratedOtp");
            string email = HttpContext.Session.GetString("Email");

            if (!string.IsNullOrEmpty(email))
            {
                var user = _context.userEntity.FirstOrDefault(u => u.Email == email && u.IsDeleted == false);
                if (user != null)
                {
                    user.PasswordHash = EncryptTool.Encrypt(request.pword);
                    _Service.UpdateUsers(user);

                    HttpContext.Session.Remove("GeneratedOtp");
                    HttpContext.Session.Remove("Email");

                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "User not found.");
                }
            }
            else
            {
                ModelState.AddModelError("", "No email found in session.");
            }

            return View(request);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetObjectAsJson("loggedUser", null);
            return RedirectToAction("Login", "Authenticate");
        }

        public async Task<IActionResult> VerifyOtp()
        {
            return View(new UserDto());
        }

        [HttpPost]
        public IActionResult VerifyOtp(string otp)
        {
            string generatedOtp = HttpContext.Session.GetString("GeneratedOtp");
            if (otp == generatedOtp)
            {
                return RedirectToAction("ResetPassword");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid OTP. Please try again.";
                return View();
            }
        }

        private string GenerateRandomOTP()
        {
            const string chars = "1234567890";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void SendEmail(string toEmail, string otp)
        {
            try
            {
                var message = new MimeKit.MimeMessage();
                message.From.Add(new MimeKit.MailboxAddress("SmartSchools", "guardianit6@gmail.com"));
                message.To.Add(new MimeKit.MailboxAddress("", toEmail));
                message.Body = new MimeKit.TextPart("plain")
                {
                    Text = $"Dear user,\n\nYour OTP is: {otp}\n\nThank you."
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    client.Authenticate("guardianit6@gmail.com", "keyzuntvhlrbehok");
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email to {toEmail}: {ex.Message}");
            }
        }
    }
}
