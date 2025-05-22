namespace SmartSchool.Models.DTO
{
    public class ClassTimetableDto
    {
        public int TimetableId { get; set; }

        public int? ClassId { get; set; }

        public int? SubjectId { get; set; }

        public int? TeacherId { get; set; }

        public string Room { get; set; }

        public string DayOfWeek { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }
        public string Class { get; set; }
        public string Subject { get; set; }
        public string FullName { get; set; }
        public string Teacher { get; set; }
    }
}
