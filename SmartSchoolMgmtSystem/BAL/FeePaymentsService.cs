using SmartSchool.Utilities;
using SmartSchool.DAL;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public class FeePaymentsService : IFeePaymentsService
    {
        private readonly IFeePaymentsRepo _feePaymentsRepo;
        public FeePaymentsService(IFeePaymentsRepo feePaymentsRepo)
        {
            _feePaymentsRepo = feePaymentsRepo;

        }
        public List<FeePaymentsDTO> GetFeePayments(int id)
        {
            return _feePaymentsRepo.GetFeePayments(id);
        }
        public GenericResponse AddFeePayments(FeePaymentsDTO obj, int id)
        {
            GenericResponse response= new GenericResponse();
           response= _feePaymentsRepo.AddFeePayments(obj,id);
            return response;
        }
        public GenericResponse UpdateFeePayments(FeePaymentsDTO obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response=_feePaymentsRepo.UpdateFeePayments(obj,id);
            return response;

        }
        public GenericResponse DeleteFeePayments(int id)
        {
            GenericResponse response = new GenericResponse();
            response = _feePaymentsRepo.DeleteFeePayments(id);
            return response;

        }
    }
}
