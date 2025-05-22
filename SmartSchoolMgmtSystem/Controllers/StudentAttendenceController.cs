using SmartSchool.Models.Entity;
using SmartSchool.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.BAL;
using SmartSchool.Models.DTO;
using Azure;

namespace SmartSchool.Controllers
{
    public class StudentAttendenceController : Controller
    {

        private readonly IStudentAttendenceServices _studentAttendenceServices;
        private readonly MyDbContext _context;
        public StudentAttendenceController(IStudentAttendenceServices StudentAttendenceServices, MyDbContext context)
        {
            _studentAttendenceServices = StudentAttendenceServices;
            _context = context;

        }
        [HttpGet]
        public IActionResult GetStudentAttendence()
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            //  ViewBag.Students = _context.studentEntity
            //.Where(s => s.IsDeleted == false)
            //.Select(s => new StudentDTO
            //{
            //    StudentId = s.StudentId,
            //     StudentName = s.StudentName  // or just s.Name if available
            //}).ToList();
            var rawClasses = _context.classEntity
            .Where(f => f.IsDeleted == false)
            .ToList();

            foreach (var c in rawClasses)
            {
                Console.WriteLine($"DEBUG: ClassId={c.ClassId}, Grade={c.Grade}, Section={c.Section}");
            }


            ViewBag.Classes = _context.classEntity
             .Where(f => f.IsDeleted == false)
             .Select(f => new ClassesDto
             {
                 ClassId = f.ClassId,
                 Grade = f.Grade,
                 Section = f.Section,
                
             }).ToList();

            ViewBag.Subjects = _context.subjectEntity
            .Where(f => f.IsDeleted == false)
            .Select(f => new SubjectDto
            {
                SubjectId = f.SubjectId,
                SubjectName = f.SubjectName,
                //  ItemName = f.ItemName // or appropriate property name
            }).ToList();

            var res = _studentAttendenceServices.GetStudentAttendence();
            return View(res);
        }

     
       
        [HttpPost]
        [Route("StudentAttendence/AddStudentAttendance")]
        public IActionResult AddStudentAttendance(List<StudentAttendenceDto> attendanceList)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            if (attendanceList == null || !attendanceList.Any())
            {
                Console.WriteLine("No data received.");
                return Json(new { success = false, message = "No Data is found" });
            }

            foreach (var attendance in attendanceList)
            {
                Console.WriteLine($"StudentId: {attendance.StudentId}, ClassId: {attendance.ClassId}, SubjectId: {attendance.SubjectId}, Date: {attendance.Date}, Status: {attendance.Status}");
            }

            // Proceed with the database insertion
            GenericResponse res = _studentAttendenceServices.AddStudentAttendances(attendanceList, loggedInUser.userId);

            if (res.statuCode == 1)
                return Json(new { success = true, message = "Attendance Added Successfully" });

            return Json(new { success = false, message = "Failed to Add Attendance" });
        }

        // Update Student Attendence
        [HttpPost]
        public IActionResult UpdateStudentAttendence(StudentAttendenceDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            GenericResponse response = _studentAttendenceServices.UpdateStudentAttendence(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetStudentAttendence");
            }
        }

        // Delete Student Attendence
        [HttpPost]
        public IActionResult DeleteStudentAttendence(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            GenericResponse response = _studentAttendenceServices.DeleteStudentAttendence(id);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetStudentAttendence");
            }
        }
        public JsonResult GetTodayAttendance(int classId)
        {
            var today = DateTime.Now.Date;
            var attendance = _context.studentattendenceEntity
                                .Where(a => a.ClassId == classId && a.Date == today)
                                .ToList();
            return Json(new { attendance });
        }
        public JsonResult GetStudentsByClass(int classId)
        {
            var students = _context.studentEntity.Where(s => s.ClassID == classId).ToList();
            return Json(new { students });
        }
        public JsonResult GetStudentsForAttendance()
        {
            try
            {
                var students = _context.studentEntity
         .Where(s => s.IsDeleted == false)
         .Select(s => new StudentDTO
         {
             StudentId = s.StudentId,
             // Name = s.Name  // or just s.Name if available
         }).ToList();// Assume you have a method that fetches all students.
                var classes = _context.classEntity
                 .Where(f => f.IsDeleted == false)
                 .Select(f => new ClassesDto
                 {
                     ClassId = f.ClassId,
                     //  ItemName = f.ItemName // or appropriate property name
                 }).ToList();// Similarly, fetch all classes.


                ViewBag.Subjects = _context.subjectEntity
                .Where(f => f.IsDeleted == false)
                .Select(f => new SubjectDto
                {
                    SubjectId = f.SubjectId,
                    //  ItemName = f.ItemName // or appropriate property name
                }).ToList();
                 

                // Return both lists as a combined result
                return Json(new { students = students, classes = classes });
            }
            catch (Exception ex)
            {
                // Log exception and return an empty response in case of error
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
