using SmartSchool.Utilities;
using SmartSchool.DAL;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public class FeeItemsService : IFeeItemsService
    {
        private readonly IFeeItemsRepo _feeItemsRepo;
        public FeeItemsService(IFeeItemsRepo feeItemsRepo)
        {
            _feeItemsRepo = feeItemsRepo;
        }
        public List<FeeItemsDto> GetFeeItems(int id)
        {
           
            return _feeItemsRepo.GetFeeItems(id);
        }
        public GenericResponse AddFeeItems(FeeItemsDto obj, int id)
        {
            GenericResponse response= new GenericResponse();
            response=_feeItemsRepo.AddFeeItems(obj, id);
            return response;
        }
        public GenericResponse UpdateFeeItems(FeeItemsDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response=_feeItemsRepo.UpdateFeeItems(obj, id);
            return response;
        }
        public GenericResponse DeleteFeeItems(int id)
        {
            GenericResponse response= new GenericResponse();
            response=_feeItemsRepo.DeleteFeeItems(id);
            return response;
        }
    }
}
