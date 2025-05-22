using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.BAL;
using OfficeOpenXml;
using Microsoft.Data.SqlClient;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;
using SmartSchool.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace SmartSchool.Controllers
{
    public class ReportCardController : Controller
    {
        private readonly IReportCardService _service;
        private readonly MyDbContext _context;
        private readonly IStudentService _studentService;

        public ReportCardController(IReportCardService service,MyDbContext context, IStudentService studentService)
        {
            _service = service;
            _context = context;
            _studentService = studentService;
        }

        [Authorize(Policy = "Teacher")]
        public IActionResult Index(int classid)
        {
            int? id = 0;
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            if (loggedInUser.userTypeName == "School Admin")
            {

                 id = _context.schools.Where(a => a.userid == loggedInUser.userId && a.IsDeleted == false).Select(a => a.SchoolId).FirstOrDefault();
                ViewBag.SchoolId = id;
                var classes = _studentService.GetClasses(loggedInUser.userId);
                ViewBag.Classes = classes;
            }
            else
            {int? uid = _context.userEntity.Where(s => s.UserId == loggedInUser.userId && s.IsDeleted == false).Select(a => a.CreatedBy).FirstOrDefault();
                id = _context.schools.Where(a => a.userid == uid&& a.IsDeleted == false).Select(a => a.SchoolId).FirstOrDefault();
                ViewBag.SchoolId = id;
                var Classes = _studentService.GetClasses(uid);
                ViewBag.Classes = Classes;
            }
            // var res = _service.GetReportCards(id,classid);
            return View();
        }

        [HttpPost]
        public IActionResult Generate(int schoolId, int classId, List<string> subjects)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            int id = _context.schools.Where(a => a.userid == loggedInUser.userId && a.IsDeleted == false).Select(a => a.SchoolId).FirstOrDefault();
            ViewBag.SchoolId = id;
            _service.GenerateReportCardTable(id, classId, subjects);
            return Json(new { message = "Table generated successfully." });
        }

        [HttpGet("GetReportCards")]
        public IActionResult GetReportCards(int schoolId, int classId)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            int id = _context.schools.Where(a => a.userid == loggedInUser.userId && a.IsDeleted == false).Select(a => a.SchoolId).FirstOrDefault();
            ViewBag.SchoolId = id;
            var reportCards = _service.GetReportCards(id, classId);
            return Json(reportCards);
        }

        [HttpPost("AddMarks")]
        public IActionResult AddMarks(int schoolId, int classId, ReportCardDto reportCard)
        {
            _service.AddStudentMarks(schoolId, classId, reportCard);
            return Json(new { message = "Marks added successfully." });
        }

        [HttpPost("EditMarks")]
        public IActionResult EditMarks(int schoolId, int classId, ReportCardDto reportCard)
        {
            _service.EditStudentMarks(schoolId, classId, reportCard);
            return Json(new { message = "Marks updated successfully." });
        }

        [HttpPost("DeleteMarks")]
        public IActionResult DeleteMarks(int schoolId, int classId, int studentId)
        {
            _service.DeleteStudentRecord(schoolId, classId, studentId);
            return Json(new { message = "Student record deleted successfully." });
        }
        [HttpGet]
        public IActionResult DownloadList(int classId)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            int id = _context.schools
                .Where(a => a.userid == loggedInUser.userId && a.IsDeleted == false)
                .Select(a => a.SchoolId)
                .FirstOrDefault();

            ViewBag.SchoolId = id;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("ReportCards");

                // Step 1: Form the table name dynamically
                string tableName = $"ReportCard_School_{id}_Class_{classId}";

                // Step 2: Fetch column names
                var columnQuery = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";
                List<string> subjects = new List<string>();

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = columnQuery;
                    _context.Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var columnName = reader.GetString(0);
                            if (columnName != "StudentName" && columnName != "Class")
                            {
                                subjects.Add(columnName);
                            }
                        }
                    }
                }

                // Step 3: Set the headers dynamically
                worksheet.Cells[1, 1].Value = "Student Name";
                worksheet.Cells[1, 2].Value = "ClassId";

                int colIndex = 3;
                foreach (var subject in subjects)
                {
                    worksheet.Cells[1, colIndex++].Value = subject;
                }

                // Step 4: Retrieve student data and populate Excel
                var studentListQuery = "SELECT StudentId, StudentName FROM Students WHERE ClassId = @classId AND IsDeleted = 0";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = studentListQuery;

                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@classId", classId));
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@schoolId", id));

                    _context.Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        int row = 2;
                        while (reader.Read())
                        {
                            worksheet.Cells[row, 1].Value = reader["StudentName"];
                            worksheet.Cells[row, 2].Value = classId;

                            colIndex = 3;
                            foreach (var subject in subjects)
                            {
                                worksheet.Cells[row, colIndex++].Value = "N/A"; // Placeholder for marks if not available
                            }
                            row++;
                        }
                    }
                }

                // Step 5: Generate Excel File
                var stream = new MemoryStream(package.GetAsByteArray());
                stream.Position = 0;
                string fileName = $"ReportCards_Class_{classId}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        
        [HttpPost]
        public IActionResult Upload()
        {
            var file = Request.Form.Files[0];
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Get the logged-in user and school info
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            int schoolId = _context.schools
                .Where(a => a.userid == loggedInUser.userId && a.IsDeleted == false)
                .Select(a => a.SchoolId)
                .FirstOrDefault();

            if (schoolId == 0)
            {
                return BadRequest("Invalid School ID.");
            }

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);

                using (var package = new ExcelPackage(stream))
                {
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                        return BadRequest("The uploaded Excel file does not contain any worksheets.");
                    }

                    var worksheet = package.Workbook.Worksheets[1];
                    if (worksheet.Dimension == null)
                    {
                        return BadRequest("The worksheet is empty.");
                    }

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    string className = worksheet.Cells[2, 2]?.Text?.Trim();
                    int classId = _context.classEntity
                        .Where(c => c.Grade == className && c.CreatedBy == loggedInUser.userId && c.IsDeleted==false)
                        .Select(c => c.ClassId)
                        .FirstOrDefault();

                    if (Convert.ToInt32(className) == 0)
                    {
                        return BadRequest("Class not found for the specified school.");
                    }

                    string tableName = $"ReportCard_School_{schoolId}_Class_{className}";

                    var columnQuery = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";
                    List<string> subjects = new List<string>();

                    using (var command = _context.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = columnQuery;
                        _context.Database.OpenConnection();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string columnName = reader.GetString(0);
                                if (columnName != "StudentId" && columnName != "StudentName")
                                {
                                    subjects.Add(columnName);
                                }
                            }
                        }
                    }

                    for (int row = 2; row <= rowCount; row++)
                    {
                        int studentId = int.Parse(worksheet.Cells[row, 3]?.Text?.Trim() ?? "0");
                        if (studentId == 0) continue;

                        List<string> updateSet = new List<string>();
                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(new SqlParameter("@studentId", studentId));

                        int colIndex = 5; 
                        foreach (var subject in subjects)
                        {
                            var markValue = worksheet.Cells[row, colIndex]?.Text?.Trim();
                            if (!string.IsNullOrEmpty(markValue))
                            {
                                updateSet.Add($"{subject} = @{subject}");
                                parameters.Add(new SqlParameter($"@{subject}", markValue));
                            }
                            colIndex++;
                        }

                        if (updateSet.Any())
                        {
                            var identityColumn = "id"; 

                            parameters = parameters.Where(p => p.ParameterName.Substring(1) != identityColumn).ToList();

                            var columnNames = parameters.Select(p => p.ParameterName.Substring(1)).ToList(); // Remove the '@' prefix
                            var parameterNames = parameters.Select(p => p.ParameterName).ToList();

                           
                            string insertQuery = $@"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) 
                        VALUES ({string.Join(", ", parameterNames)})";

                            using (var command = _context.Database.GetDbConnection().CreateCommand())
                            {
                                command.CommandText = insertQuery;
                                command.Parameters.AddRange(parameters.ToArray());
                                _context.Database.OpenConnection();
                                command.ExecuteNonQuery();
                            }

                        }
                    }
                }
            }

            return Json(new { message = "File uploaded and data updated successfully." });
        }
        private List<ReportCardDto> GetReportCards(int classId)
        {
            // Placeholder: Replace with database fetching logic
            return new List<ReportCardDto>
            {
                new ReportCardDto { StudentId = 1, StudentName = "John Doe", Class = "10-A", Subjects = new List<string> { "Math", "English" } },
                new ReportCardDto { StudentId = 2, StudentName = "Jane Smith", Class = "10-A", Subjects = new List<string> { "Science", "History" } }
            };
        }

    }
}




       
//        [HttpGet]
//        public IActionResult GetReportCards(int classId)
//        {
//            var reportCards = GetReportCards(classId); // Placeholder for database fetch
//            return Json(reportCards);
//        }

//   
//    }
//}
