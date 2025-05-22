
using SmartSchool.Utilities;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;

namespace SmartSchool.DAL
{
    public class StudentAttendenceRepo : IStudentAttendenceRepo
    {
   
        private readonly MyDbContext _context;
        private readonly IConfiguration _config;
        public StudentAttendenceRepo(MyDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

       


        public List<StudentAttendenceDto> GetStudentAttendence()
        {
            //var result = (from StudentAttendence in _context.studentattendenceEntity
            //              where StudentAttendence.IsDeleted == false 
            //              select new StudentAttendenceDto
            //              {
            //                  AttendanceId = StudentAttendence.AttendanceId,
            //                  StudentId = StudentAttendence.StudentId,
            //                  StudentName =  _context.studentEntity.Where(a => a.StudentName == StudentAttendence.StudentName && a.IsDeleted == false).Select(a => (string?)a.StudentName).FirstOrDefault(),
            //                  ClassId = StudentAttendence.ClassId,
            //                  ClassName= _context.classEntity.Where(a => a.ClassName == StudentAttendence.ClassName && a.IsDeleted == false).Select(a => (string?)a.ClassName).FirstOrDefault(),
            //                  SubjectId = StudentAttendence.SubjectId,
            //                  SubjectName= _context.subjectEntity.Where(a => a.SubjectName == StudentAttendence.SubjectName && a.IsDeleted == false).Select(a => (string?)a.SubjectName).FirstOrDefault(),
            //                  Date = StudentAttendence.Date,
            //                  Status = StudentAttendence.Status
            //              }).ToList();

            //return result;
            var result = (from attendence in _context.studentattendenceEntity
                          join student in _context.studentEntity on attendence.StudentId equals student.StudentId
                          join classes in _context.classEntity on attendence.ClassId equals classes.ClassId
                          join subject in _context.subjectEntity on attendence.SubjectId equals subject.SubjectId
                          where attendence.IsDeleted == false
                                && student.IsDeleted == false
                                && classes.IsDeleted == false
                                && subject.IsDeleted == false
                          select new StudentAttendenceDto
                          {
                              AttendanceId = attendence.AttendanceId,
                              StudentId = attendence.StudentId,
                              StudentName = student.StudentName,
                              ClassId = attendence.ClassId,
                             // ClassName = classes.ClassName,
                              SubjectId = attendence.SubjectId,
                              SubjectName = subject.SubjectName,
                              Date = attendence.Date,
                              Status = attendence.Status
                          }).ToList();

            return result;
        }


        public GenericResponse AddStudentAttendances(List<StudentAttendenceDto> attendanceList, int id)
        {
             GenericResponse response = new GenericResponse();
            try
           { int aid = _context.studentattendenceEntity.OrderByDescending(a => a.AttendanceId).Select(a => a.AttendanceId).FirstOrDefault();
                foreach (var attendance in attendanceList)
            {
                var entity = new StudentAttendenceEntity
                {AttendanceId = aid+1,
                    StudentId = attendance.StudentId,
                    ClassId = attendance.ClassId,
                    SubjectId = attendance.SubjectId,
                    Date = attendance.Date,
                    Status = attendance.Status,
                    IsDeleted = false,
                     CreatedOn = DateTime.Now,
                    CreatedBy = id,
                    UpdatedOn = DateTime.Now,
                    UpdatedBy = id,
                  
                };
                    aid = entity.AttendanceId;
                _context.studentattendenceEntity.Add(entity);
               
            }
                _context.SaveChanges();
                // var check = _context.studentattendenceEntity.Where(a => a.AttendanceId == entity.AttendanceId).FirstOrDefault();
                response.statuCode = 1;
                response.message = "Success";
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to add StudentAttendence" + ex;
            }
            return response;
        }

        public GenericResponse UpdateStudentAttendence(StudentAttendenceDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.studentattendenceEntity.Where(a => a.AttendanceId == obj.AttendanceId && a.IsDeleted == false).FirstOrDefault();
            int count = _context.studentattendenceEntity.Where(a => a.AttendanceId == obj.AttendanceId).Count();
            try
            {
                if (count == 1)
                {
                    result.AttendanceId = obj.AttendanceId;
                    result.StudentId = obj.StudentId;
                    result.ClassId = obj.ClassId;
                    result.SubjectId = obj.SubjectId;
                    result.Date = DateTime.Now;
                    result.Status = obj.Status;
                    result.IsDeleted = false;
                    result.CreatedOn = result.CreatedOn;
                    result.CreatedBy = result.CreatedBy;
                    result.UpdatedOn = DateTime.Now;
                    result.UpdatedBy = id;
                    

                    _context.studentattendenceEntity.Update(result);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = result.AttendanceId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "StudentAttendence Alredy exist";
                }

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to UpdateStudentAttendence" + ex;
            }
            return response;
        }

        public GenericResponse DeleteStudentAttendence(int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.studentattendenceEntity.Where(a => a.AttendanceId == id && a.IsDeleted == false).FirstOrDefault();
            try
            {
                result.IsDeleted = true;
                _context.studentattendenceEntity.Update(result);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Success";
                response.currentId = result.AttendanceId;

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Delete StudentAttendence" + ex;
            }
            return response;
        }

    } 
}

