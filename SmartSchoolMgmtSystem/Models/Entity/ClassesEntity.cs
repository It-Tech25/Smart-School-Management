using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class ClassesEntity
    {
        [Key]
        public int ClassId { get; set; }

        //public string ClassName { get; set; }


        public string Grade { get; set; }

        public string Section { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

    }
}
