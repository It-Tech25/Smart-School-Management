using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.DAL
{
    public interface IClassesRepo
    {
        List<ClassesDto> GetClasses(int id);
        GenericResponse AddClass(ClassesDto obj, int id);
        GenericResponse UpdateClass(ClassesDto obj, int id);
        GenericResponse DeleteClass(int id);
    }
}
