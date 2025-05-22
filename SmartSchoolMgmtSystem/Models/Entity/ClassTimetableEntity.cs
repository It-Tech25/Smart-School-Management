using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class ClassTimetableEntity
    {
        [Key]
        public int TimetableId { get; set; }

        public int? ClassId { get; set; }

        public int? SubjectId { get; set; }

        public int? TeacherId { get; set; }

        public string Room { get; set; }

        public string DayOfWeek { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
