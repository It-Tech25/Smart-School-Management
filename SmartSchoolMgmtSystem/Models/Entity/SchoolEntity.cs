using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class SchoolEntity
    {
        [Key]
        public int SchoolId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public int? userid { get; set; }

        public string? ProfilePhoto1 { get; set; }

        public string? ProfilePhoto2 { get; set; }

        public string? ProfilePhoto3 { get; set; }


    }
}
