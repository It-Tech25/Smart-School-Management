using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public interface IAdmissionsServices
    {
        List<string> GetStates();
        List<string> Getcity(string state);
        // Get all Admnissions
        List<AdmissionsDto> GetAllAdmissions();
        // Add a new Admissions
        GenericResponse AddAdmission(AdmissionsDto obj, int id);
        // Update admissions
        GenericResponse UpdateAdmissions(AdmissionsDto obj, int id);
        // Delete admissions
        GenericResponse DeleteAdmissions(int id);
    }
}
