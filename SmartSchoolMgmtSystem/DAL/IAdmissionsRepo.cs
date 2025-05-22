using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.DAL
{
    public interface IAdmissionsRepo
    {
        List<string> GetStates();
        List<string> Getcity(string state);
        List<AdmissionsDto> GetAdmissions();
        GenericResponse AddAdmission(AdmissionsDto obj, int UserId);
        GenericResponse UpdateAdmission(AdmissionsDto obj, int userid);
        GenericResponse DeleteAdmission(int id);
    }
}
