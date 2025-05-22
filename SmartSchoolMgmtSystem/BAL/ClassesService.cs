using SmartSchool.DAL;
using SmartSchool.Models.Entity;
using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public class ClassesService: IClassesService
    {
        private readonly IClassesRepo _ClassRepo;
        private readonly MyDbContext _context;
        private readonly IConfiguration _config;


        public ClassesService(IClassesRepo ClassRepo, MyDbContext contex, IConfiguration config)
        {
            _ClassRepo = ClassRepo;
            _context = contex;
            _config = config;
        }
        public List<ClassesDto> GetClass(int id)
        {
            return _ClassRepo.GetClasses(id);
        }
        public GenericResponse AddClass(ClassesDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response = _ClassRepo.AddClass(obj, id);
            return response;
        }
        public GenericResponse UpdateClass(ClassesDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response = _ClassRepo.UpdateClass(obj, id);
            return response;
        }
        public GenericResponse DeleteClass(int id)
        {
            GenericResponse response = new GenericResponse();
            response = _ClassRepo.DeleteClass(id);
            return response;
        }


    }
}
