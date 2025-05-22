using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class SubscriptionPaymentsEntity
    {
        [Key]

        public int PaymentId { get; set; }

        public int? SubscriptionId { get; set; }

        public decimal? Amount { get; set; }

        public DateTime? PaidDate { get; set; }

        public string Status { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? Enddate { get; set; }

        public int? schoolid { get; set; }

        public int? Durationid { get; set; }

        public string Modules { get; set; }
    }
}
