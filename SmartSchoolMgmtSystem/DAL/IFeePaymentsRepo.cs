using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.DAL
{
    public interface IFeePaymentsRepo
    {
        List<FeePaymentsDTO> GetFeePayments(int id);
        GenericResponse AddFeePayments(FeePaymentsDTO obj, int id);
        GenericResponse UpdateFeePayments(FeePaymentsDTO obj, int id);
        GenericResponse DeleteFeePayments(int id);

    }
}
