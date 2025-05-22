using SmartSchool.Utilities;
using SmartSchool.DAL;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public class ClassTimetableServices : IClassTimetableServices
    {
        private readonly IClassTimetableRepo _ClassTimetableRepo;

        public ClassTimetableServices(IClassTimetableRepo ClassTimetableRepo)
        {
            _ClassTimetableRepo = ClassTimetableRepo;
        }
        public List<string> GetStates()
        {
            return _ClassTimetableRepo.GetStates();
        }
        public List<string> Getcity(string state)
        {
            return _ClassTimetableRepo.Getcity(state);
        }
        // Get all ClassTimetable
        public List<ClassTimetableDto> GetAllClassTimetable()
        {
            return _ClassTimetableRepo.GetClassTimetable();
        }

        // Add a new ClassTimetable
        public GenericResponse AddClassTimetable(ClassTimetableDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response = _ClassTimetableRepo.AddClassTimetable(obj, id);
            return response;
        }

        // Update ClassTimetable
        public GenericResponse UpdateClassTimetable(ClassTimetableDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response = _ClassTimetableRepo.UpdateClassTimetable(obj, id);
            return response;
        }

        // Delete ClassTimetable
        public GenericResponse DeleteClassTimetable(int id)
        {
            GenericResponse response = new GenericResponse();
            response = _ClassTimetableRepo.DeleteClassTimetable(id);
            return response;
        }
    }
}
