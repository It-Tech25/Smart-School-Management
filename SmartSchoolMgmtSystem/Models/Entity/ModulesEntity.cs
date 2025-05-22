using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class ModulesEntity
    {
        [Key]
        public int ModuleId { get; set; }

        public string Name { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

    }
}
