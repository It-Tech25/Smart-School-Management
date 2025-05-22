using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.DAL
{
    public interface IClassTimetableRepo
    {
        List<string> GetStates();
        List<string> Getcity(string state);
        List<ClassTimetableDto> GetClassTimetable();
        GenericResponse AddClassTimetable(ClassTimetableDto obj, int id);
        GenericResponse UpdateClassTimetable(ClassTimetableDto obj, int id);
        GenericResponse DeleteClassTimetable(int id);
    }
}
