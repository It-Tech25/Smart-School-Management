using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class AddressEntity
    {
        [Key]
        public int AddressId { get; set; }

        public int? UserId { get; set; }

        public string AddressLine { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PinCode { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
