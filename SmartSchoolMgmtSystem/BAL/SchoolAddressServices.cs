using SmartSchool.BAL;
using SmartSchool.DAL;
using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public class SchoolAddressServices:ISchoolAddressServices
    {
            private readonly ISchoolAddressRepo _schoolAddressRepo;

            public SchoolAddressServices(ISchoolAddressRepo schoolAddressRepo)
            {
                _schoolAddressRepo = schoolAddressRepo;
            }
        public List<string> GetStates()
        {
            return _schoolAddressRepo.GetStates();
        }
        public List<string> Getcity(string state)
        {
            return _schoolAddressRepo.Getcity(state);
        }
        // Get all school addresses
        public List<SchoolAddressDto> GetAllSchoolAddresses()
            {
                return _schoolAddressRepo.GetSchoolAddress();
            }

            // Add a new school address
            public GenericResponse AddSchoolAddress(SchoolAddressDto obj, int id)
            {
                GenericResponse response = new GenericResponse();
                response = _schoolAddressRepo.AddSchoolAddress(obj, id);
                return response;
            }

            // Update school address
            public GenericResponse UpdateSchoolAddress(SchoolAddressDto obj, int id)
            {
                GenericResponse response = new GenericResponse();
                response = _schoolAddressRepo.UpdateSchoolAddress(obj, id);
                return response;
            }

            // Delete school address
            public GenericResponse DeleteSchoolAddress(int id)
            {
                GenericResponse response = new GenericResponse();
                response = _schoolAddressRepo.DeleteSchoolAddress(id);
                return response;
            }
        }
    
}

