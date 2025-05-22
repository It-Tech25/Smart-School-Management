
using SmartSchool.Models.Entity;
using SmartSchool.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.BAL;
using SmartSchool.Models.DTO;
using System.Text.RegularExpressions;
using static SmartSchool.BAL.SchoolServices;
using Microsoft.AspNetCore.Authorization;

namespace SmartSchool.Controllers
{
    public class SchoolController : Controller
    {
        private readonly IschoolServices _school;
        private readonly MyDbContext _context;
        private readonly ISchoolAddressServices _schoolAddressService;
        public SchoolController(IschoolServices school, ISchoolAddressServices schoolAddressService, MyDbContext context)
        {
            _school = school;
            _schoolAddressService = schoolAddressService;
            _context = context;
        }
        [Authorize(Policy = "Super Admin")]
        public IActionResult GetSchool(string? state)
        {
            var states = _schoolAddressService.GetStates();
            ViewBag.State = states;
            var cities = _schoolAddressService.Getcity(state);
            ViewBag.city = cities;
            ViewBag.Principle = _context.userEntity.Where(a => a.IsDeleted == false && a.UserTypeId == 2).Select(a => a.FullName).ToList();
            var res = _school.GetAllSchools();
            return View(res);
        }
        [HttpPost]
        public IActionResult AddSchool(IFormCollection form, IFormFile ProfilePhoto1, IFormFile ProfilePhoto2, IFormFile ProfilePhoto3)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            GenericResponse response = new GenericResponse();
            var user = new SchoolDto
            {
                Name = form["Name"],
                Code = form["Code"],
                UserName = form["UserName"],
                AddressType = form["AddressType"],
                AddressLine = form["AddressLine"],
                State = form["State"],
                City = form["City"],
                ZipCode = form["ZipCode"]
            };

            // Create the directory if it doesn't exist
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Handle ProfilePhoto1
            if (ProfilePhoto1 != null && ProfilePhoto1.Length > 0)
            {
                var fileName1 = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePhoto1.FileName);
                var filePath1 = Path.Combine(uploadPath, fileName1);

                using (var stream = new FileStream(filePath1, FileMode.Create))
                {
                    ProfilePhoto1.CopyTo(stream);
                }

                user.ProfilePhoto1 = "/uploads/" + fileName1;
            }

            // Handle ProfilePhoto2
            if (ProfilePhoto2 != null && ProfilePhoto2.Length > 0)
            {
                var fileName2 = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePhoto2.FileName);
                var filePath2 = Path.Combine(uploadPath, fileName2);

                using (var stream = new FileStream(filePath2, FileMode.Create))
                {
                    ProfilePhoto2.CopyTo(stream);
                }

                user.ProfilePhoto2 = "/uploads/" + fileName2;
            }

            // Handle ProfilePhoto3
            if (ProfilePhoto3 != null && ProfilePhoto3.Length > 0)
            {
                var fileName3 = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePhoto3.FileName);
                var filePath3 = Path.Combine(uploadPath, fileName3);

                using (var stream = new FileStream(filePath3, FileMode.Create))
                {
                    ProfilePhoto3.CopyTo(stream);
                }

                user.ProfilePhoto3 = "/uploads/" + fileName3;
            }

            // Call the service to save the data
            response = _school.AddSchool(user, loggedInUser.userId);
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
        public IActionResult UpdateSchool(IFormCollection form, IFormFile ProfilePhoto1, IFormFile ProfilePhoto2, IFormFile ProfilePhoto3)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            GenericResponse response = new GenericResponse();

            // Fetch the existing school data to retain current values for fields not being updated
            var schoolId = Convert.ToInt32(form["SchoolId"]);
            var existingSchool = _context.schools.Where(a => a.SchoolId == schoolId && a.IsDeleted == false).FirstOrDefault();

            if (existingSchool == null)
            {
                return Json(new { success = false, message = "School not found." });
            }

            var user = new SchoolDto
            {
                SchoolId =Convert.ToInt32(form["SchoolId"]),
                Name = form["Name"],
                Code = form["Code"],
                UserName = form["UserName"],
                AddressType = form["AddressType"],
                AddressLine = form["AddressLine"],
                State = form["State"],
                City = form["City"],
                ZipCode = form["ZipCode"]
            };

            // Create the directory if it doesn't exist
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Handle ProfilePhoto1
            if (ProfilePhoto1 != null && ProfilePhoto1.Length > 0)
            {
                var fileName1 = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePhoto1.FileName);
                var filePath1 = Path.Combine(uploadPath, fileName1);

                using (var stream = new FileStream(filePath1, FileMode.Create))
                {
                    ProfilePhoto1.CopyTo(stream);
                }

                user.ProfilePhoto1 = "/uploads/" + fileName1;
            }

            // Handle ProfilePhoto2
            if (ProfilePhoto2 != null && ProfilePhoto2.Length > 0)
            {
                var fileName2 = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePhoto2.FileName);
                var filePath2 = Path.Combine(uploadPath, fileName2);

                using (var stream = new FileStream(filePath2, FileMode.Create))
                {
                    ProfilePhoto2.CopyTo(stream);
                }

                user.ProfilePhoto2 = "/uploads/" + fileName2;
            }

            // Handle ProfilePhoto3
            if (ProfilePhoto3 != null && ProfilePhoto3.Length > 0)
            {
                var fileName3 = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePhoto3.FileName);
                var filePath3 = Path.Combine(uploadPath, fileName3);

                using (var stream = new FileStream(filePath3, FileMode.Create))
                {
                    ProfilePhoto3.CopyTo(stream);
                }

                user.ProfilePhoto3 = "/uploads/" + fileName3;
            }


            // Call the service to update the data
            response = _school.UpdateSchool(user, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Update failed." });
            }
        }
        [HttpPost]
        public IActionResult DeleteSchool(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();
            response = _school.DeleteSchool(id);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetUserType");
            }
        }
        [HttpGet]
        public JsonResult GetCitiesByState(string state)
        {
            var cities = _schoolAddressService.Getcity(state);

            return Json(cities);
        }

        [HttpGet]
        public JsonResult GetSchoolById(int id)
        {
            var school = _context.SchoolAddresses
     .Where(s => s.SchoolId == id)
     .Select(s => new SchoolDto
     {
         SchoolId = s.SchoolId ?? 0,
         Name = _context.schools.Where(a => a.SchoolId == s.SchoolId && a.IsDeleted == false).Select(a => a.Name).FirstOrDefault(), // assuming the field is Name
         Code = _context.schools.Where(a => a.SchoolId == s.SchoolId && a.IsDeleted == false).Select(a => a.Code).FirstOrDefault(),

         AddressType = s.AddressType,
         AddressLine = s.AddressLine,
         State = s.State,
         City = s.City,
         ZipCode = s.ZipCode
     })
     .FirstOrDefault();


            if (school == null)
            {
                return Json(new { success = false, message = "School not found" });
            }

            return Json(new { success = true, data = school });
        }

    }
}