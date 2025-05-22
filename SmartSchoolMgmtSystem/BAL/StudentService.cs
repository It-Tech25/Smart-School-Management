using SmartSchool.Utilities;
using SmartSchool.DAL;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepo _studentRepo;
        public StudentService(IStudentRepo studentRepo)
        {
            _studentRepo = studentRepo;
        }
        public List<ClassesDto>GetClasses(int? id)
        {
            return _studentRepo.GetClasses(id);
        }
        public List<StudentDTO> GetStudent(int id)
        {
           return  _studentRepo.GetStudent(id);
        }
        public GenericResponse AddStudent(StudentDTO obj, int id)
        {
           GenericResponse response= new GenericResponse();
           response= _studentRepo.AddStudent(obj, id);
            return response;
        }
        public GenericResponse UpdateStudent(StudentDTO obj, int id)
        {
            GenericResponse response= new GenericResponse();
            response=_studentRepo.UpdateStudent(obj, id);
            return response;
        }
        public GenericResponse DeleteStudent(int id)
        {
            GenericResponse response= new GenericResponse();
           response= _studentRepo.DeleteStudent(id);
            return response;
        }

    }
}
