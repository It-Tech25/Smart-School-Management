using SmartSchool.DAL;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;
using SmartSchool.Utilities;

namespace SmartSchool.BAL
{
    public class ClassDurationService: IClassDurationService
    {
        private readonly IClassDurationRepo _classrepo;
        

        public ClassDurationService(IClassDurationRepo classrepo)
        {
            _classrepo = classrepo;
        }
        public List<ClassDurationDto> Classtiming(int id)
        {
            var result = _classrepo.Classtiming(id);

            return result;
        }

        public GenericResponse AddClasstiming(ClassDurationDto obj, int id)
        {
            var response = _classrepo.AddClasstiming(obj, id);
            return response;
        }

        public GenericResponse UpdateClasstiming(ClassDurationDto obj, int id)
        {
            var response = _classrepo.UpdateClasstiming(obj, id);
            return response;


        }
        public GenericResponse DeleteClassDuration(int id)
        {
            var response = _classrepo.DeleteClassDuration(id);
            return response;
        }

    }

}
