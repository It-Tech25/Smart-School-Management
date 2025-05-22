namespace SmartSchool.Models.DTO
{
    public class AdmissionsDto
    {
        public int AdmissionId { get; set; }

        public string StudentName { get; set; }

        public int? SchoolId { get; set; }
        public int? UserTypeId { get; set; }
        public string AdharcardNo { get; set; }
        public string? Photo { get; set; }
        public int? ClassId { get; set; }
        public decimal? TotalFee { get; set; }
        public decimal? AdmissionFee { get; set; }
        public string AcademicYear { get; set; }
        public string School { get; set; }
        public string Class { get; set; }
        public string? Address { get; set; }

        public DateTime? DOB { get; set; }

        public string? Gender { get; set; }
        public bool? IsAdded { get; set; }

    }
  
}
