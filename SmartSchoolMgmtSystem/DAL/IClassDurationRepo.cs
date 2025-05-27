using SmartSchool.Models.DTO;
using SmartSchool.Utilities;

namespace SmartSchool.DAL
{
    public interface IClassDurationRepo
    {
        List<ClassDurationDto> Classtiming(int id);
        GenericResponse AddClasstiming(ClassDurationDto obj, int id);
        GenericResponse UpdateClasstiming(ClassDurationDto obj, int id);
        GenericResponse DeleteClassDuration(int id);
    }
}
