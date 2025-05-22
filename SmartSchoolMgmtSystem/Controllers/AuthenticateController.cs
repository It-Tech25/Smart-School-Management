using SmartSchool.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SmartSchool.BAL;
using SmartSchool.Models.Entity;
using SmartSchool.Models.DTO;

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


        public IActionResult Login()
        {
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

                // If the login attempt is unsuccessful, show error
                if (result.statusCode == 0)
                {
                    ViewBag.ErrorMessage = result.Message;
                    return View(request);
                }

                // Assign claims based on the login result
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, result.userId.ToString()), // User ID claim
            new Claim(ClaimTypes.Name, result.userName), // User name claim
            new Claim("UserType", result.userTypeName) // Custom claim for user type
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                // Sign the user in with the claims
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                              new ClaimsPrincipal(claimsIdentity), authProperties);

                // Store user information in session
                HttpContext.Session.SetString("UserId", result.userId.ToString());
                HttpContext.Session.SetString("UserName", result.userName);
                HttpContext.Session.SetObjectAsJson("loggedUser", result);

                // Redirect based on the user type
                return result.userTypeName switch
                {
                    "Super Admin" => RedirectToAction("Index", "Home"),
                    "School Admin" => RedirectToAction("AdminDashboard", "Home"),
                    "Teacher" => RedirectToAction("TeacherDashboard", "Home")
                };
            }

            // If validation fails, return to login view
            return View(request);
        }


        public async Task<IActionResult> ForgotPassword()
        {
            UserDto r = new UserDto();
            return View(r);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(UserDto request)
        {

            var user = await _Service.GetUserByEmailAsync(request.Email);


            if (user == null || user.IsDeleted==true)
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
            ResetPasswordFinal request = new ResetPasswordFinal();

            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordFinal request)
        {
            string generatedOtp = HttpContext.Session.GetString("GeneratedOtp");


            string email = HttpContext.Session.GetString("Email");

            if (!string.IsNullOrEmpty(email))
            {
                var user = _context.userEntity
                    .FirstOrDefault(u => u.Email == email && u.IsDeleted == false);

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
            UserDto request = new UserDto();

            return View(request);
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
            const string chars = "1234567890";  // OTP will consist of digits only
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());  // Generate a 4-digit OTP
        }

        private void SendEmail(string toEmail, string otp)
        {
            try
            {
                var message = new MimeKit.MimeMessage();
                message.From.Add(new MimeKit.MailboxAddress("SmartSchools", "guardianit6@gmail.com")); // Replace with your email
                message.To.Add(new MimeKit.MailboxAddress("", toEmail));
                //message.Subject = "Welcome to Our Platform";.

                message.Body = new MimeKit.TextPart("plain")
                {
                    Text = $"Dear user,\n\nYour account has been created successfully. Here is your OTP for login: {otp}\n\nThank you."
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);  // Replace with your SMTP server and port
                    client.Authenticate("guardianit6@gmail.com", "keyzuntvhlrbehok"); // Replace with your credentials

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
