using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public interface IClassTimetableServices
    {
        List<string> GetStates();
        List<string> Getcity(string state);
        // Get all ClassTimetable
        List<ClassTimetableDto> GetAllClassTimetable();
        // Add a new ClassTimetable
        GenericResponse AddClassTimetable(ClassTimetableDto obj, int id);
        // Update ClassTimetable
        GenericResponse UpdateClassTimetable(ClassTimetableDto obj, int id);
        // Delete ClassTimetable
        GenericResponse DeleteClassTimetable(int id);
    }
}
