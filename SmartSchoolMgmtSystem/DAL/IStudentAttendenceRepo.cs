using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.DAL
{
    public interface IStudentAttendenceRepo
    {
        List<StudentAttendenceDto> GetStudentAttendence();
        GenericResponse AddStudentAttendances(List<StudentAttendenceDto> attendanceList, int id);
        GenericResponse UpdateStudentAttendence(StudentAttendenceDto obj, int id);
        GenericResponse DeleteStudentAttendence(int id);
    }
}
