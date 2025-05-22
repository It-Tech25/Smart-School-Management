using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class StateEntity
    {
        [Key]
        public int StateId { get; set; }
        public string StateName { get; set; }
    }
}
