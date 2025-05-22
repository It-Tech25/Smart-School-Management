using SmartSchool.Utilities;
using SmartSchool.DAL;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;
using OfficeOpenXml.FormulaParsing.Utilities;

namespace SmartSchool.BAL
{
    public class StudentAttendenceServices : IStudentAttendenceServices
    {
        private readonly IStudentAttendenceRepo _studentAttendenceRepo;

        public StudentAttendenceServices(IStudentAttendenceRepo studentAttendenceRepo)
        {
            _studentAttendenceRepo = studentAttendenceRepo;
        }

        // Get all Student Attendence
        public List<StudentAttendenceDto> GetStudentAttendence()
        {
            return _studentAttendenceRepo.GetStudentAttendence();
        }

        // Add a new Student Attendence
        public GenericResponse AddStudentAttendances(List<StudentAttendenceDto> attendanceList,int id )
        {
            GenericResponse res = new GenericResponse();

            res= _studentAttendenceRepo.AddStudentAttendances(attendanceList,id);
            return res;
        }

        // Update Student Attendence
        public GenericResponse UpdateStudentAttendence(StudentAttendenceDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response = _studentAttendenceRepo.UpdateStudentAttendence(obj, id);
            return response;
        }

        // Delete Student Attendence
        public GenericResponse DeleteStudentAttendence(int id)
        {
            GenericResponse response = new GenericResponse();
            response = _studentAttendenceRepo.DeleteStudentAttendence(id);
            return response;
        }
    }
}
