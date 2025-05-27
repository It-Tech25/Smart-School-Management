using SmartSchool.Models.DTO;
using SmartSchool.Utilities;

namespace SmartSchool.BAL
{
    public interface IClassDurationService
    {
        List<ClassDurationDto> Classtiming(int id);
        GenericResponse AddClasstiming(ClassDurationDto obj, int id);
        GenericResponse UpdateClasstiming(ClassDurationDto obj, int id);
        GenericResponse DeleteClassDuration(int id);
    }
}
