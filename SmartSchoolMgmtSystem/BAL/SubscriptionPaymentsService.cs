using SmartSchool.Utilities;
using Microsoft.EntityFrameworkCore;
using SmartSchool.DAL;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;

namespace SmartSchool.BAL
{
    public class SubscriptionPaymentsService: ISubscriptionPaymentsService
    {
        private readonly ISubscriptionPaymentsRepo _subscriptionrepo;
        public SubscriptionPaymentsService(ISubscriptionPaymentsRepo subscriptionrepo)
        {
            _subscriptionrepo = subscriptionrepo;
        }
        public List<SubscriptionPaymentsDto> GetPaymentDetails()
        {
            var result = _subscriptionrepo.GetPaymentDetails();
            return result;
        }


        public GenericResponse AddPayment(SubscriptionPaymentsDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response = _subscriptionrepo.AddPayment(obj,id);
            return response;
        }

        public GenericResponse UpdatePayment(SubscriptionPaymentsDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response = _subscriptionrepo.UpdatePayment(obj, id);
            return response;
        }

        public GenericResponse DeletePayment(int id)
        {
            GenericResponse response = new GenericResponse();
            response = _subscriptionrepo.DeletePayment(id);
            return response;
        }

    }
}
