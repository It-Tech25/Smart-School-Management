using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.DAL
{
    public interface IFeeCategoriesRepo
    {
        List<FeeCatagoriesDto> GetFeeCategoryName(int id);
        GenericResponse AddFeeCategory(FeeCatagoriesDto obj, int id);
        GenericResponse UpdateFeeCategory(FeeCatagoriesDto obj, int id);
        GenericResponse DeleteFeeCategory(int id);
    }
}
