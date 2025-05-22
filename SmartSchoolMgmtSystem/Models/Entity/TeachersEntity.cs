using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class TeachersEntity
    {
        [Key]
        public int TeacherId { get; set; }

        public int? UserId { get; set; }

        public int? SchoolId { get; set; }
        public int? ClassId { get; set; }

        public int? DirectionId { get; set; }


        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public int? SubjectId { get; set; }

    }
}
