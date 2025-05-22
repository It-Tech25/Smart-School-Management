namespace SmartSchool.Models.DTO
{
    public class ReportCardDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Class { get; set; }
        public List<string> Subjects { get; set; }
        public Dictionary<string, int> Marks { get; set; }
    }

}
