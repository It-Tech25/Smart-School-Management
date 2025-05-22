using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public interface IFeeCategoriesService
    {
        List<FeeCatagoriesDto> GetFeeCategoryName(int id);
        GenericResponse AddFeeCategory(FeeCatagoriesDto obj, int id);
        GenericResponse UpdateFeeCategory(FeeCatagoriesDto obj, int id);
        GenericResponse DeleteFeeCategory(int id);
    }
}
