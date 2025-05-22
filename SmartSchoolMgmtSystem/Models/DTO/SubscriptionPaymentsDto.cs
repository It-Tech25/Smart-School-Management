namespace SmartSchool.Models.DTO
{
    public class SubscriptionPaymentsDto
    {
        public int PaymentId { get; set; }

        public int? SubscriptionId { get; set; }

        public decimal? Amount { get; set; }

        public DateTime? PaidDate { get; set; }

        public int? schoolid { get; set; }

        public int? Durationid { get; set; }

        public string Modules { get; set; }
        public string Status { get; set; }
        public DateTime? Enddate { get; set; }
        public string SubscriptionType { get; set; }
        public List<string> SelectedModules { get; set; }
        public string User { get; set; }
        public string School { get; set; }
        public string Duration { get; set; }
        public int? userId { get; set; }
    }
}
