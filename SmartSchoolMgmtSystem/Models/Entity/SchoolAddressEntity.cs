using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class SchoolAddressEntity
    {
        [Key]
        public int AddressId { get; set; }

        public int? SchoolId { get; set; }

        public string AddressType { get; set; }

        public string AddressLine { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
