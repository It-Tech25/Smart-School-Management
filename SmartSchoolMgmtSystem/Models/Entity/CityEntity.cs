using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class CityEntity
    {
        [Key]
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int StateId { get; set; }
    }
}
