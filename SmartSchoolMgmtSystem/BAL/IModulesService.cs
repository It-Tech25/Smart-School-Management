using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public interface IModulesService
    {
        List<ModulesDto> GetModule(int id);
        GenericResponse AddModule(ModulesDto obj, int id);
        GenericResponse UpdateModule(ModulesDto obj, int id);
        GenericResponse DeleteModule(int id);
    }
}
