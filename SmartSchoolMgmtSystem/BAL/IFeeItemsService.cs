using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public interface IFeeItemsService
    {
        List<FeeItemsDto> GetFeeItems(int id);
        GenericResponse AddFeeItems(FeeItemsDto obj, int id);
        GenericResponse UpdateFeeItems(FeeItemsDto obj, int id);
        GenericResponse DeleteFeeItems(int id);
    }
}
