using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.DAL
{
    public interface IStudentRepo
    {
        List<ClassesDto> GetClasses(int? id);
        List<StudentDTO> GetStudent(int id);
        GenericResponse AddStudent(StudentDTO obj, int id);
        GenericResponse UpdateStudent(StudentDTO obj, int id);
        GenericResponse DeleteStudent(int id);

    }
}
