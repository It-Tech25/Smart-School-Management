using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartSchool.Models.Entity
{
    public class StudentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }

        [Required]
        public int? SchoolId { get; set; }
        [Required]
        public int? ClassID { get; set; }
        //  public string? Class { get; set; } 

        [Required]
        public string? Address { get; set; }
        [Required]
        public DateTime? DOB { get; set; }
        [Required]
        public string Gender { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
        [Required]
        public string? StudentName { get; set; }
        [Required]
        public int? UserTypeId { get; set; }
        [Required]
        public string? AdharcardNo { get; set; }
        [Required]
        public string? Photo { get; set; }
       
        public decimal? TotalFee { get; set; }
     
        public decimal? PaiedFee { get; set; }
        public bool? IsAdded { get; set; }

        public virtual List<StudentFamily> Family { get; set; } = new List<StudentFamily>();
    }
}
