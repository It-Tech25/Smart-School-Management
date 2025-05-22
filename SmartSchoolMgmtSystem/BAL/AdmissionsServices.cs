using SmartSchool.Utilities;
using SmartSchool.DAL;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public class AdmissionsServices : IAdmissionsServices
    {
        private readonly IAdmissionsRepo _AdmisssionsRepo;

        public AdmissionsServices(IAdmissionsRepo AdmissionsRepo)
        {
            _AdmisssionsRepo = AdmissionsRepo;
        }
        public List<string> GetStates()
        {
            return _AdmisssionsRepo.GetStates();
        }
        public List<string> Getcity(string state)
        {
            return _AdmisssionsRepo.Getcity(state);
        }
        // Get all Admnissions
        public List<AdmissionsDto> GetAllAdmissions()
        {
            return _AdmisssionsRepo.GetAdmissions();
        }

        // Add a new Admissions
        public GenericResponse AddAdmission(AdmissionsDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response = _AdmisssionsRepo.AddAdmission(obj, id);
            return response;
        }

        // Update admissions
        public GenericResponse UpdateAdmissions(AdmissionsDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response = _AdmisssionsRepo.UpdateAdmission(obj, id);
            return response;
        }

        // Delete admissions
        public GenericResponse DeleteAdmissions(int id)
        {
            GenericResponse response = new GenericResponse();
            response = _AdmisssionsRepo.DeleteAdmission(id);
            return response;
        }
    }

}
