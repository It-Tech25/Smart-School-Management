using SmartSchool.BAL;
using SmartSchool.Models.Entity;
using SmartSchool.Utilities;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BAL;
using SmartSchool.Models.DTO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace SmartSchool.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly IUserService _user;
        private readonly MyDbContext _context;
        private readonly ISchoolAddressServices _schoolAddressService;
        public UserManagementController(MyDbContext context,IUserService user,ISchoolAddressServices schoolAddressServices)
        {
            _user = user;
            _context = context;
            _schoolAddressService = schoolAddressServices;
        }
        [Authorize(Policy = "Super Admin,School Admin")]
        public IActionResult GetUserType()
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
           
            var res = _user.GetUserType(loggedInUser.userId);
            return View(res);
        }
        [HttpPost]
        public IActionResult AddUserType(UserTypeDTO obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _user.AddUserType(obj,loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetUserType");
            }
        }
        [HttpPost]
        public IActionResult UpdateUserType(UserTypeDTO obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _user.UpdateUserType(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetUserType");
            }
        }
        [HttpPost]
        public IActionResult DeleteUserType(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();
            response = _user.DeleteUserType(id);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetUserType");
            }
        }

        //UserMaster
        public IActionResult GetUser(string? state )
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            var states = _schoolAddressService.GetStates();
            ViewBag.State = states;
            var cities = _schoolAddressService.Getcity(state);
            ViewBag.city = cities;
            var usertype = _context.userTypeEntites.Where(a => a.IsDeleted == false && a.CreatedBy==loggedInUser.userId).Select(a => a.UserTypeName).ToList();
            ViewBag.UserType = usertype;
            var res = _user.GetUser(loggedInUser.userId);
            return View(res);
        }
        public IActionResult AddUser(IFormCollection form, IFormFile ProfilePicture)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            //string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            //var email = form["Email"].ToString().Trim();

            //if (!Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase))
            //{
            //    return Json(new { success = false, message ="Enter a valid Email Id" }); 
            //}

            var user = new UserDto
            {
                FullName = form["FullName"],
                Email = form["Email"],
                UserType = form["UserType"],
                PasswordHash = form["PasswordHash"],
                  AddressLine = form["AddressLine"],
                State = form["State"],
                City = form["City"],
                PinCode = form["PinCode"]

            };

            if (ProfilePicture != null && ProfilePicture.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePicture.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                     ProfilePicture.CopyToAsync(stream);
                }

                user.ProfilePicture = "/uploads/" + fileName;
            }

            var result = _user.AddUser(user,loggedInUser.userId);
            bool success = result != null;
            return Json(new { success = success, message = success ? null : "User could not be added" });
        }

        [HttpPost]
        public IActionResult UpdateUser(IFormCollection form, IFormFile ProfilePicture)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            var email = form["Email"].ToString().Trim();

            if (!Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase))
            {
                return Json(new { success = false, message = "Enter a valid Email Id" });
            }

            var user = new UserDto
            {
                UserId = Convert.ToInt32(form["UserId"]),
                FullName = form["FullName"],
                Email = email,
                UserType = form["UserType"],
                PasswordHash = form["PasswordHash"]
            };

            if (ProfilePicture != null && ProfilePicture.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePicture.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ProfilePicture.CopyTo(stream); // Note: Use CopyTo (sync) since you're not awaiting
                }

                user.ProfilePicture = "/uploads/" + fileName;
            }

            var result = _user.UpdateUser(user, loggedInUser.userId);

            if (result != null && result.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = result?.message ?? "User could not be updated" });
            }
        }
        public IActionResult DeleteUser(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();
            response = _user.DeleteUser(id);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetUser");
            }
        }

        [HttpGet]
        public IActionResult GetCitiesByState(string state)
        {

            var cities = _schoolAddressService.Getcity(state);
            return Json(cities);
        }
    }
}
