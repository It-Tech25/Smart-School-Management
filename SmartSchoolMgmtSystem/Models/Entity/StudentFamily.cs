using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartSchool.Models.Entity
{
    public class StudentFamily
    {
        [Key]
        public int FamilyId { get; set; }

        public int? StudentId { get; set; }
        [ForeignKey("StudentId")]
        public virtual StudentEntity Student { get; set; }

        public string Name { get; set; }

        public string Relation { get; set; }

        public string ContactNumber { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

    }
}
