using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class FeePaymentEntity
    {
        [Key]
        public int Paymentid { get; set; }

        public int? StudentId { get; set; }

        public int? ItemId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public decimal? Amount { get; set; }

        public string ReceiptNumber { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
