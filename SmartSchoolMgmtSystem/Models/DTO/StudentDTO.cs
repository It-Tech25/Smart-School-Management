using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.DTO
{
    public class StudentDTO
    {
        

        public int StudentId { get; set; }
        [Required]
        public int? ClassID { get; set; }

        [Required]
        public DateTime? DOB { get; set; }
        [Required]
        public string? Gender { get; set; }
        [Required]
        public string? StudentName { get; set; }
        [Required]
        public int? UserTypeId { get; set; }
        [Required]
        public string? AdharcardNo { get; set; }
        [Required]
        public string? Photo { get; set; }
        [Required]
        public int? SchoolId { get; set; }
        [Required]
        public decimal? TotalFee { get; set; }
        [Required]

        public bool? IsAdded { get; set; }
       
        public decimal? PaiedFee { get; set; }
        public string Address { get; set; }
        public string Class { get; set; }
        public int FamilyId { get; set; }


        public string Name { get; set; }

        public string Relation { get; set; }

        public string ContactNumber { get; set; }

        public StudentFamilyDto sfamily = new StudentFamilyDto();

    }
}
