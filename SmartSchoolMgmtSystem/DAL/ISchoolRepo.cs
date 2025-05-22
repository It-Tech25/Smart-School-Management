using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.DAL
{
    public interface ISchoolRepo
    {
        List<SchoolDto> GetSchool();

        GenericResponse AddSchool(SchoolDto obj, int id);

        GenericResponse UpdateSchool(SchoolDto obj, int id);

        GenericResponse DeleteSchool(int id);
    }
}
