using SmartSchool.Models.Entity;

namespace SmartSchool.Models.DTO
{
    public class FeeItemsDto
    {
        public int ItemId { get; set; }

        public int? CategoryId { get; set; }

        public string Name { get; set; }

        public decimal? Amount { get; set; }
       

    }
}
