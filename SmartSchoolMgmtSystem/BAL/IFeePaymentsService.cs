using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public interface IFeePaymentsService
    {
        List<FeePaymentsDTO> GetFeePayments(int id);
        GenericResponse AddFeePayments(FeePaymentsDTO obj, int id);
        GenericResponse UpdateFeePayments(FeePaymentsDTO obj, int id);
        GenericResponse DeleteFeePayments(int id);

    }
}
