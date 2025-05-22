using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public interface IStudentAttendenceServices
    {
        // Get all Student Attendence
        List<StudentAttendenceDto> GetStudentAttendence();
        // Add a new Student Attendence
        GenericResponse AddStudentAttendances(List<StudentAttendenceDto> attendanceList, int id);
        // Update Student Attendence
        GenericResponse UpdateStudentAttendence(StudentAttendenceDto obj, int id);
        // Delete Student Attendence
        GenericResponse DeleteStudentAttendence(int id);
    }
}
