
using SmartSchool.Utilities;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;

namespace SmartSchool.DAL
{
    public class SubscriptionPaymentsRepo: ISubscriptionPaymentsRepo
    {
        private readonly MyDbContext _context;
        public SubscriptionPaymentsRepo(MyDbContext context)
        {
            _context = context;
        }
        public List<SubscriptionPaymentsDto> GetPaymentDetails()
        {
            var result = (from user in _context.subscriptionsPaymentEntity
                          where user.IsDeleted == false
                          select new SubscriptionPaymentsDto
                          {
                              PaymentId=user.PaymentId,
                              SubscriptionType = _context.subscriptionsTypeEntity
                                  .Where(a => a.SubscriptionTypeId == user.SubscriptionId && a.IsDeleted == false)
                                  .Select(a => a.ModulesEnabled)
                                  .FirstOrDefault(),

                              Amount = user.Amount,
                              PaidDate = user.PaidDate,
                              Status = user.Status,
                              Enddate = user.Enddate,
                              Modules = _context.subscriptionsTypeEntity.Where(a => a.SubscriptionTypeId == user.SubscriptionId && a.IsDeleted == false).Select(a => a.ModulesEnabled).FirstOrDefault(),
                              

                              
                          }).ToList();

            return result;
        }



        public GenericResponse AddPayment(SubscriptionPaymentsDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            SubscriptionPaymentsEntity entity = new SubscriptionPaymentsEntity();
            int c = 0;
            int Paymentid = _context.subscriptionsPaymentEntity.OrderByDescending(a => a.PaymentId).Select(a => a.PaymentId).FirstOrDefault();
            try
            {
               
                int count = _context.subscriptionsPaymentEntity
                    .Where(a => a.SubscriptionId == obj.SubscriptionId &&
                                a.CreatedOn == DateTime.Now.Date &&
                                a.Status == "Active" &&
                                a.IsDeleted == false)
                    .Count();

                if (count == 0)
                {
                    entity.SubscriptionId = obj.SubscriptionId;
                    entity.Amount = obj.Amount;
                    entity.schoolid = _context.schools.Where(s => s.Name == obj.School).Select(a=>a.SchoolId).FirstOrDefault();
                    entity.Durationid = _context.duration.Where(s => s.DurationType == obj.Duration).Select(a => a.Durationid).FirstOrDefault();
                    foreach(var i in obj.SelectedModules)
                    {
                        entity.Modules = entity.Modules + "," + i;
                    }
                    entity.PaidDate = obj.PaidDate ?? DateTime.Now;
                    entity.PaymentId = Paymentid + 1;
                    entity.Enddate = obj.Enddate;
                    entity.Status = "Active";
                    entity.IsDeleted = false;
                    entity.CreatedOn = DateTime.Now;
                    entity.CreatedBy = id;

                    _context.subscriptionsPaymentEntity.Add(entity);
                    if (c == 0)
                    {
                        _context.SaveChanges();
                        c++;
                        response.statuCode = 1;
                        response.message = "Success";
                        response.currentId = entity.PaymentId;
                    }
                 
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Payment Already Done";
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to add Payment: " + ex.Message;
            }

            return response;
        }

        public GenericResponse UpdatePayment(SubscriptionPaymentsDto obj, int id)
        {
            GenericResponse response = new GenericResponse();

            try
            {
                var entity = _context.subscriptionsPaymentEntity
                    .FirstOrDefault(p => p.PaymentId == obj.PaymentId && p.IsDeleted == false);

                if (entity == null)
                {
                    response.statuCode = 0;
                    response.message = "Payment record not found.";
                    return response;
                }


                entity.SubscriptionId = obj.SubscriptionId;
                entity.Amount = obj.Amount;
                entity.schoolid = _context.schools.Where(s => s.Name == obj.School).Select(a => a.SchoolId).FirstOrDefault();
                entity.Durationid = _context.duration.Where(s => s.DurationType == obj.Duration).Select(a => a.Durationid).FirstOrDefault();
                foreach (var i in obj.SelectedModules)
                {
                    entity.Modules = entity.Modules + "," + i;
                }
                entity.PaidDate = obj.PaidDate ?? DateTime.Now;
                entity.Enddate = obj.Enddate;
                entity.Status = obj.Status ?? "Active";
                entity.UpdatedOn = DateTime.Now;
                entity.UpdatedBy = id;
                _context.subscriptionsPaymentEntity.Update(entity);
                _context.SaveChanges();

                response.statuCode = 1;
                response.message = "Payment updated successfully.";
                response.currentId = entity.PaymentId;
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to update payment: " + ex.Message;
            }

            return response;
        }

        public GenericResponse DeletePayment(int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.subscriptionsPaymentEntity.FirstOrDefault(p => p.PaymentId == id&& p.IsDeleted == false);
            try
            {
                result.IsDeleted = true;
                _context.subscriptionsPaymentEntity.Update(result);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Success";
                response.currentId = result.PaymentId;

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Delete UserType" + ex;
            }
            return response;
        }


    }
}
