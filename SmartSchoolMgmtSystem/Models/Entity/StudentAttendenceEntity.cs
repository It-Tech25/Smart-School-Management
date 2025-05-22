using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class StudentAttendenceEntity
    {
        [Key]
        public int AttendanceId { get; set; }

        public int? StudentId { get; set; }
        //public string StudentName { get; set; }

        //public string ClassName { get; set; }

        //public string SubjectName { get; set; }

        public int? ClassId { get; set; }

        public int? SubjectId { get; set; }

        public DateTime? Date { get; set; }

        public string Status { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

    }
}
