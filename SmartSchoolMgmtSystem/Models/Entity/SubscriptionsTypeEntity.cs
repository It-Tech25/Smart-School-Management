using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class SubscriptionsTypeEntity
    {
        [Key]
        public int SubscriptionTypeId { get; set; }

        public string ModulesEnabled { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

    }
}
