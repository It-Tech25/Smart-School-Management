using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.DTO
{
    public class StudentDTO
    {
        

        public int StudentId { get; set; }
        [Required]
        public int? ClassID { get; set; }
         
        public DateTime? DOB { get; set; }
  
        public string? Gender { get; set; }
         
        public string? StudentName { get; set; }
         
        public int? UserTypeId { get; set; }
       
        public string? AdharcardNo { get; set; }
        
        public string? Photo { get; set; }
         
        public int? SchoolId { get; set; }
        
        public decimal? TotalFee { get; set; }
       

        public bool? IsAdded { get; set; }
       
        public decimal? PaiedFee { get; set; }
        public string? Address { get; set; }
        public string? Class { get; set; }
        public int? FamilyId { get; set; }


        public string? Name { get; set; }

        public string? Relation { get; set; }

        public string? ContactNumber { get; set; }

        public StudentFamilyDto sfamily = new StudentFamilyDto();

    }
}
