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
    
        public int? ClassID { get; set; }
        //  public string? Class { get; set; } 
         
        public string? Address { get; set; }
        
        public DateTime? DOB { get; set; }
        
        public string? Gender { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
        
        public string? StudentName { get; set; }
        
        public int? UserTypeId { get; set; }
        
        public string? AdharcardNo { get; set; }
        
        public string? Photo { get; set; }
       
        public decimal? TotalFee { get; set; }
     
        public decimal? PaiedFee { get; set; }
        public bool? IsAdded { get; set; }

        public virtual List<StudentFamily> Family { get; set; } = new List<StudentFamily>();
    }
}
