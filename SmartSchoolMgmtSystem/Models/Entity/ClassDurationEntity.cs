using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class ClassDurationEntity
    {
        [Key]
    public int ClassDurationId { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public int? Duration { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

    }


}
