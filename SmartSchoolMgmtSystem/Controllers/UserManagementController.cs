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

        public UserManagementController(MyDbContext context, IUserService user, ISchoolAddressServices schoolAddressServices)
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
                return Json(new { success = false, message = "Session expired" });
            }

            try
            {
                GenericResponse response = _user.AddUserType(obj, loggedInUser.userId);
                if (response.statuCode == 1)
                {
                    return Json(new { success = true, message = response.message });
                }
                else
                {
                    return Json(new { success = false, message = response.message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult UpdateUserType(UserTypeDTO obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Session expired" });
            }

            try
            {
                GenericResponse response = _user.UpdateUserType(obj, loggedInUser.userId);
                if (response.statuCode == 1)
                {
                    return Json(new { success = true, message = response.message });
                }
                else
                {
                    return Json(new { success = false, message = response.message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteUserType(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Session expired" });
            }

            try
            {
                GenericResponse response = _user.DeleteUserType(id);
                if (response.statuCode == 1)
                {
                    return Json(new { success = true, message = response.message });
                }
                else
                {
                    return Json(new { success = false, message = response.message ?? "Failed to delete user type" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        // UserMaster
        public IActionResult GetUser(string? state)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            List<string> userTypes = new List<string>();

            if (loggedInUser.userTypeName == "Super Admin")
            {
                userTypes.Add("School Admin");
            }
            else if (loggedInUser.userTypeName == "School Admin")
            {
                userTypes.AddRange(new[] { "Teacher", "Student" });
            }
            else if (loggedInUser.userTypeName == "Teacher")
            {
                userTypes.Add("Student");
            }

            ViewBag.UserType = userTypes;

            var states = _schoolAddressService.GetStates();
            ViewBag.State = states;
            var cities = _schoolAddressService.Getcity(state);
            ViewBag.city = cities;

            var res = _user.GetUser(loggedInUser.userId);
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(IFormCollection form, IFormFile ProfilePicture)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Session expired" });
            }

            try
            {
                // Email validation
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                var email = form["Email"].ToString().Trim();

                if (!Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase))
                {
                    return Json(new { success = false, message = "Enter a valid Email Id" });
                }

                var user = new UserDto
                {
                    FullName = form["FullName"],
                    Email = email,
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

                    // Ensure directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfilePicture.CopyToAsync(stream);
                    }

                    user.ProfilePicture = "/uploads/" + fileName;
                }

                var result = _user.AddUser(user, loggedInUser.userId);
                if (result != null && result.statuCode == 1)
                {
                    return Json(new { success = true, message = result.message });
                }
                else
                {
                    return Json(new { success = false, message = result?.message ?? "User could not be added" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(IFormCollection form, IFormFile ProfilePicture)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Session expired" });
            }

            try
            {
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

                    // Ensure directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfilePicture.CopyToAsync(stream);
                    }

                    user.ProfilePicture = "/uploads/" + fileName;
                }

                var result = _user.UpdateUser(user, loggedInUser.userId);

                if (result != null && result.statuCode == 1)
                {
                    return Json(new { success = true, message = result.message });
                }
                else
                {
                    return Json(new { success = false, message = result?.message ?? "User could not be updated" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteUser(int UserId)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Session expired" });
            }

            // Validate UserId parameter
            if (UserId <= 0)
            {
                return Json(new { success = false, message = "Invalid User ID provided" });
            }

            try
            {
                GenericResponse response = _user.DeleteUser(UserId);
                if (response.statuCode == 1)
                {
                    return Json(new { success = true, message = response.message });
                }
                else
                {
                    return Json(new { success = false, message = response.message ?? "Failed to delete user" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        // Alternative delete method that accepts different parameter names
        [HttpPost]
        public IActionResult DeleteUserById(int id)
        {
            return DeleteUser(id);
        }

        // GET method for debugging - you can remove this after testing
        [HttpGet]
        public IActionResult CheckUser(int id)
        {
            try
            {
                var user = _context.userEntity.Where(u => u.UserId == id).FirstOrDefault();
                if (user == null)
                {
                    return Json(new { exists = false, message = "User not found" });
                }
                return Json(new
                {
                    exists = true,
                    userId = user.UserId,
                    fullName = user.FullName,
                    isDeleted = user.IsDeleted,
                    isActive = user.IsActive
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetCitiesByState(string state)
        {
            try
            {
                var cities = _schoolAddressService.Getcity(state);
                return Json(cities);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to get cities: " + ex.Message });
            }
        }
    }
}