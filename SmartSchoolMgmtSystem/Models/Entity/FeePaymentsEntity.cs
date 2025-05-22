using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartSchool.Models.Entity
{
    public class FeePaymentsEntity
    {
        [Key]
        public int PaymentId { get; set; }

        public int? StudentId { get; set; }

        public int? ItemId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public decimal? Amount { get; set; }

        public string? ReceiptNumber { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
        [ForeignKey("StudentId")]
        public virtual StudentEntity Student { get; set; }

        [ForeignKey("ItemId")]
        public virtual FeeItemsEntity FeeItem { get; set; }

    }
}
