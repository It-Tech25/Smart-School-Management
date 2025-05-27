namespace SmartSchool.Models.DTO
{
    public class ClassDurationDto
    {
        public int ClassDurationId { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public int? Duration { get; set; }

    }
}
