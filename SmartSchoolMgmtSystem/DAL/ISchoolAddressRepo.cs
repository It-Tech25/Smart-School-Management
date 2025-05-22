using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.DAL
{
    public interface ISchoolAddressRepo
    {
        List<string> GetStates();
        List<string> Getcity(string state);
        List<SchoolAddressDto> GetSchoolAddress();

        GenericResponse AddSchoolAddress(SchoolAddressDto obj, int id);
        GenericResponse UpdateSchoolAddress(SchoolAddressDto obj, int id);
        GenericResponse DeleteSchoolAddress(int id);
    }
}
