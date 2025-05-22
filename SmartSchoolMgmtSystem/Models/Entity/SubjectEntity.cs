using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class SubjectEntity
    {
        [Key]
        public int SubjectId { get; set; }

        public string SubjectName { get; set; }

        public string Description { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
