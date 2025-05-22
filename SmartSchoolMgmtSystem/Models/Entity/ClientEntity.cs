using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class ClientEntity
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? FullName { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? Company { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
