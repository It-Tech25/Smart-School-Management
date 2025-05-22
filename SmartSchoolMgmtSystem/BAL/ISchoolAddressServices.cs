using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public interface ISchoolAddressServices
    {
        List<string> GetStates();
        List<string> Getcity(string state);
        List<SchoolAddressDto> GetAllSchoolAddresses();
        GenericResponse AddSchoolAddress(SchoolAddressDto obj, int id);
        GenericResponse UpdateSchoolAddress(SchoolAddressDto obj, int id);
        GenericResponse DeleteSchoolAddress(int id);
    }
}
