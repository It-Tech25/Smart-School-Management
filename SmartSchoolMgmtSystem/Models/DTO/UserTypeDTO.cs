using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.DTO
{
    public class UserTypeDTO
    {
        public int UserTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? UserTypeName { get; set; }
        public string? Description { get; set; }

    }
}
