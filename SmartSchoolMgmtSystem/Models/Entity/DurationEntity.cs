using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class DurationEntity
    {
        [Key]
        public int Durationid { get; set; }

        public string DurationType { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

    }
}
