namespace SmartSchool.Models.Entity
{
    public class MasterReportCard
    {
        public int Id { get; set; }
        public int SchoolId { get; set; }
        public int ClassId { get; set; }
        public string TableName { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }

}
