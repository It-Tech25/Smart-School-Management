using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public interface IClassesService
    {
        List<ClassesDto> GetClass(int id);
        GenericResponse AddClass(ClassesDto obj, int id);
        GenericResponse UpdateClass(ClassesDto obj, int id);
        GenericResponse DeleteClass(int id);
    }
}
