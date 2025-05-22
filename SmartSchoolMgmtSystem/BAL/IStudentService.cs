using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public interface IStudentService
    {
        List<ClassesDto> GetClasses(int? id);
        List<StudentDTO> GetStudent(int id);
        GenericResponse AddStudent(StudentDTO obj, int id);
        GenericResponse UpdateStudent(StudentDTO obj, int id);
        GenericResponse DeleteStudent(int id);

    }
}
