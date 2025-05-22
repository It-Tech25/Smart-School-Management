using SmartSchool.Utilities;
using SmartSchool.DAL;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public class SchoolServices : IschoolServices
	{
       
            private readonly ISchoolRepo _schoolRepo;

            public SchoolServices(ISchoolRepo schoolRepo)
            {
                _schoolRepo = schoolRepo;
            }

            // Get all schools
            public List<SchoolDto> GetAllSchools()
            {
                return _schoolRepo.GetSchool();
            }

            // Add a new school
            public GenericResponse AddSchool(SchoolDto obj, int id)
            {
                GenericResponse response = new GenericResponse();
                response = _schoolRepo.AddSchool(obj, id);
                return response;
            }

            // Update school
            public GenericResponse UpdateSchool(SchoolDto obj, int id)
            {
                GenericResponse response = new GenericResponse();
                response = _schoolRepo.UpdateSchool(obj, id);
                return response;
            }

            // Delete school
            public GenericResponse DeleteSchool(int id)
            {
                GenericResponse response = new GenericResponse();
                response = _schoolRepo.DeleteSchool(id);
                return response;
            }

            
        }
    
    
}
