namespace SmartSchool.Models.DTO
{
    public class SchoolDto
    {
        public int SchoolId { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }
        public string? UserName{ get; set; }

        public bool? IsActive { get; set; }
        public string? ProfilePhoto1 { get; set; }

        public string? ProfilePhoto2 { get; set; }

        public string? ProfilePhoto3 { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? userid { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
        public string? School { get; set; }
        public string? AddressType { get; set; }

        public string? AddressLine { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? ZipCode { get; set; }


    }
}
