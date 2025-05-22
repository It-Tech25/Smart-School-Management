using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.DAL
{
    public interface ISubscriptionPaymentsRepo
    {
        List<SubscriptionPaymentsDto> GetPaymentDetails();
        GenericResponse AddPayment(SubscriptionPaymentsDto obj, int id);
        GenericResponse UpdatePayment(SubscriptionPaymentsDto obj, int id);
        GenericResponse DeletePayment(int id);
    }
}
