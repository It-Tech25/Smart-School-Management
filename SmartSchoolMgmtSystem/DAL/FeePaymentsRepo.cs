using SmartSchool.Models.Entity;
using SmartSchool.Utilities;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Models.DTO;

namespace SmartSchool.DAL
{
    public class FeePaymentsRepo : IFeePaymentsRepo
    {
   
     
        private readonly MyDbContext _context;
        private readonly IConfiguration _config;

        public FeePaymentsRepo(MyDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public List<FeePaymentsDTO> GetFeePayments(int id)
        {
            var result = (from user in _context.feePaymentEntity
                          where user.IsDeleted == false && user.CreatedBy == id
                          select new FeePaymentsDTO
                          {
                              PaymentId = user.PaymentId,
                              StudentId = user.StudentId,
                              ItemId=user.ItemId,
                              Name = _context.studentEntity.Where(a => a.StudentId == user.StudentId && a.IsDeleted == false).Select(a => a.StudentName).FirstOrDefault(),
                              Item = _context.feeItemEntity.Where(a => a.ItemId == user.ItemId && a.IsDeleted == false).Select(a => a.Name).FirstOrDefault(),
                              PaymentDate = user.PaymentDate,
                              Amount = user.Amount,
                              ReceiptNumber = user.ReceiptNumber,


                          }).ToList();

            return result;
        }
        public GenericResponse AddFeePayments(FeePaymentsDTO obj, int id)
        {
            GenericResponse response = new GenericResponse();
            FeePaymentsEntity entity = new FeePaymentsEntity();

         

             int iid = _context.feeItemEntity.Where(a => a.ItemId == obj.ItemId && a.IsDeleted == false).Select(a => a.ItemId).FirstOrDefault();
             int sid = _context.studentEntity.Where(a => a.StudentId == obj.StudentId && a.IsDeleted == false).Select(a => a.StudentId).FirstOrDefault();
            int count = _context.feePaymentEntity.Where(a => a.StudentId == obj.StudentId && a.CreatedBy == id && a.IsDeleted == false).Count();
          
            try
            {
                if (count ==0)
                {
                    entity.StudentId = sid;
                    entity.ItemId = iid;
                    entity.PaymentDate= DateTime.Now;
                    entity.Amount=obj.Amount;
                    entity.ReceiptNumber= GenerateReceiptNumber();
                    entity.IsDeleted = false;
                    entity.CreatedOn = DateTime.Now;
                    entity.CreatedBy = id;
                    entity.UpdatedOn= DateTime.Now;
                    entity.UpdatedBy= id;


                    _context.feePaymentEntity.Add(entity);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = entity.PaymentId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Fee Payments Alredy exist";
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to add Fee Payments" + ex;
            }
            return response;
        }
        public GenericResponse UpdateFeePayments(FeePaymentsDTO obj, int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.feePaymentEntity.Where(a => a.StudentId == obj.StudentId&& a.PaymentId==obj.PaymentId && a.IsDeleted == false).FirstOrDefault();
            int sid = _context.studentEntity.Where(a => a.StudentId == obj.StudentId && a.IsDeleted == false).Select(a => a.StudentId).FirstOrDefault();  
            int count = _context.feePaymentEntity.Where(a => a.PaymentId == obj.PaymentId).Count();
            try
            {
                if (count <= 1)
                {
                    result.PaymentId = obj.PaymentId;
                    result.StudentId = sid;
                    result.ItemId = obj.ItemId;
                    result.PaymentDate= DateTime.Now;
                    result.Amount = result.Amount+obj.Amount;
                    result.ReceiptNumber= GenerateReceiptNumber();
                    result.IsDeleted = false;
                    result.CreatedOn = result.CreatedOn;
                    result.CreatedBy= result.CreatedBy;
                    result.UpdatedOn = DateTime.Now;
                    result.UpdatedBy = id;

                    _context.feePaymentEntity.Update(result);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = result.PaymentId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Fee Payments Alredy exist";
                }

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to Update Fee Payments" + ex;
            }
            return response;

        }
        public GenericResponse DeleteFeePayments(int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.feePaymentEntity.Where(a => a.PaymentId == id && a.IsDeleted == false).FirstOrDefault();
            try
            {
                result.IsDeleted = true;
                _context.feePaymentEntity.Update(result);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Success";
                response.currentId = result.PaymentId;

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Delete Fee Payments" + ex;
            }
            return response;
        }

        private string GenerateReceiptNumber()
        {
            return "RCPT-" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }


    }
}
