using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.DAL
{
    public interface IFeeItemsRepo
    {
        List<FeeItemsDto> GetFeeItems(int id);
        GenericResponse AddFeeItems(FeeItemsDto obj, int id);
        GenericResponse UpdateFeeItems(FeeItemsDto obj, int id);
        GenericResponse DeleteFeeItems(int id);
    }
}
