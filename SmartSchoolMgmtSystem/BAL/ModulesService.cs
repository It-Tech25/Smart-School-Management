using SmartSchool.Utilities;
using SmartSchool.DAL;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public class ModulesService : IModulesService
    {
        private readonly IModulesRepo _modulesRepo;
        public ModulesService(IModulesRepo modulesRepo)
        {
            _modulesRepo=modulesRepo;

        }
        public List<ModulesDto> GetModule(int id)
        {
           return _modulesRepo.GetModule(id);

        }
        public GenericResponse AddModule(ModulesDto obj, int id)
        {
            GenericResponse response=new GenericResponse();
            response=_modulesRepo.AddModule(obj, id);
            return response;
        }
        public GenericResponse UpdateModule(ModulesDto obj, int id)
        {
            GenericResponse response=new GenericResponse();
            response=_modulesRepo.UpdateModule(obj, id);
            return response;
        }
        public GenericResponse DeleteModule(int id)
        {
             GenericResponse response= new GenericResponse();
            response=_modulesRepo.DeleteModule(id);
            return response;
        }


    }
}
