using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public interface IschoolServices
    {
        List<SchoolDto> GetAllSchools();

        GenericResponse AddSchool(SchoolDto obj, int id);

        GenericResponse UpdateSchool(SchoolDto obj, int id);
        GenericResponse DeleteSchool(int id);
    }
}
