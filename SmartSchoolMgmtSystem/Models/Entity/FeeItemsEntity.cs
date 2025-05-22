using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.Entity
{
    public class FeeItemsEntity
    {
        [Key]
        public int ItemId { get; set; }

        public int? CategoryId { get; set; }

        public string Name { get; set; }

        public decimal? Amount { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
       
    }
}
