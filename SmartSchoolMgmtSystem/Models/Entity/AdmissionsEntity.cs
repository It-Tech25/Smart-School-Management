using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartSchool.Models.Entity
{
    public class AdmissionsEntity
    {
        [Key]
        public int AdmissionId { get; set; }

        public int? SchoolId { get; set; }

        public int? ClassId { get; set; }

        public string? AcademicYear { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }
        public int? UserTypeId { get; set; }
        public string? AdharcardNo { get; set; }
        public string? Photo { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public string? StudentName { get; set; }
        public decimal? TotalFee { get; set; }

        public decimal? AdmissionFee { get; set; }
        public string? Address { get; set; }
       
        public DateTime? DOB { get; set; }
      
        public string? Gender { get; set; }

        public bool? IsAdded { get; set; }

    }
}
