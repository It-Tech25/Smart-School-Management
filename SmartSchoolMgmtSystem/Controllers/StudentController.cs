using SmartSchool.Models.Entity;
using SmartSchool.Utilities;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BAL;
using SmartSchool.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace SmartSchool.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly MyDbContext _context;

        public StudentController(IStudentService studentService, MyDbContext context)
        {
            _studentService = studentService;
            _context = context;
        }
        [Authorize(Policy = "School Admin")]
        public IActionResult GetStudents()
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            var classes = _studentService.GetClasses(loggedInUser.userId);
            ViewBag.Classes = classes;
            var res = _studentService.GetStudent(loggedInUser.userId);
            return View(res);

        }
        [HttpPost]
        public IActionResult AddStudent(IFormCollection form, IFormFile Photo)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();
            var user = new StudentDTO
            {
                StudentName = form["StudentName"],
                ClassID = Convert.ToInt32(form["ClassID"]),
                //AddressId = Convert.ToInt32(form["AddressId"]),
                DOB = Convert.ToDateTime(form["DOB"]),
                Gender = form["Gender"],
                UserTypeId = Convert.ToInt32(form["UserTypeId"]),
                AdharcardNo = form["AdharcardNo"],

                SchoolId = Convert.ToInt32(form["SchoolId"]),
                TotalFee = Convert.ToDecimal(form["TotalFee"]),
                Address = form["Address"],
                Class = form["Class"],
                Name = form["Name"],
                Relation = form["Relation"],
                ContactNumber = form["ContactNumber"],
            };

            // Create the directory if it doesn't exist
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Handle ProfilePhoto1
            if (Photo != null && Photo.Length > 0)
            {
                var fileName1 = Guid.NewGuid().ToString() + Path.GetExtension(Photo.FileName);
                var filePath1 = Path.Combine(uploadPath, fileName1);

                using (var stream = new FileStream(filePath1, FileMode.Create))
                {
                    Photo.CopyTo(stream);
                }

                user.Photo = "/uploads/" + fileName1;

                response = _studentService.AddStudent(user, loggedInUser.userId);
                if (response.statuCode == 1)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            return View();
        }
        [HttpPost]
        public IActionResult UpdateStudent(IFormCollection form, IFormFile Photo)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            GenericResponse response = new GenericResponse();

            var user = new StudentDTO
            {
                StudentId = Convert.ToInt32(form["StudentId"]),
                StudentName = form["StudentName"],
                ClassID = Convert.ToInt32(form["ClassID"]),
                DOB = Convert.ToDateTime(form["DOB"]),
                Gender = form["Gender"],
                UserTypeId = Convert.ToInt32(form["UserTypeId"]),
                AdharcardNo = form["AdharcardNo"],
                SchoolId = Convert.ToInt32(form["SchoolId"]),
                TotalFee = Convert.ToDecimal(form["TotalFee"]),
                Address = form["Address"],
                Class = form["Class"],
                Name = form["Name"],
                Relation = form["Relation"],
                ContactNumber = form["ContactNumber"]
            };

            // Check if there is a new photo to update
            if (Photo != null && Photo.Length > 0)
            {
                // Create the directory if it doesn't exist
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Photo.FileName);
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Photo.CopyTo(stream);
                }

                // Update the user's photo path
                user.Photo = "/uploads/" + fileName;
            }

            // Call the service to update the student
            response = _studentService.UpdateStudent(user, loggedInUser.userId);
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
        public IActionResult DeleteStudent(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            GenericResponse response = new GenericResponse();

            response = _studentService.DeleteStudent(id);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [HttpGet]
        public JsonResult GetAddressByFullName(string fullName)
        {
            int id = _context.studentEntity.Where(a => a.StudentName == fullName && a.IsDeleted == false).Select(a => a.StudentId).FirstOrDefault();
            var address = _context.addressEntity
                .Where(a => a.UserId == id)
                .Select(a => a.AddressLine)
                .FirstOrDefault();

            return Json(new { address = address });
        }
        [HttpGet]
        public IActionResult GetAddressByName(string fullName)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            int id = _context.studentEntity.Where(a => a.StudentName == fullName && a.CreatedBy == loggedInUser.userId).Select(a => a.StudentId).FirstOrDefault();
            var address = _context.addressEntity
                .Where(s => s.UserId == id)
                .Select(s => s.AddressLine)
                .FirstOrDefault();

            if (address != null)
            {
                return Json(new { address });
            }

            return Json(new { address = string.Empty });
        }


    }
}
