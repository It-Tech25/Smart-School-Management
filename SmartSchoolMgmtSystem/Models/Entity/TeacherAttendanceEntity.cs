using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class TeacherAttendanceEntity
    {
        [Key]
        public int AttendanceId { get; set; }

        public int TeacherId { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? ClockIn { get; set; }

        public TimeSpan? ClockOut { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string UpdatedBy { get; set; }
    }
}
