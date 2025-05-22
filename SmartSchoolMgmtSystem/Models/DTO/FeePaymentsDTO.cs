namespace SmartSchool.Models.DTO
{
    public class FeePaymentsDTO
    {
        public int PaymentId { get; set; }

        public int? StudentId { get; set; }

        public int? ItemId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public decimal? Amount { get; set; }

        public string? ReceiptNumber { get; set; }
        public string? Name { get; set; }
        public string? Item { get; set; }

    }
}
