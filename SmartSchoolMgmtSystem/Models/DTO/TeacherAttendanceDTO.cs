namespace SmartSchool.Models.DTO
{
    public class TeacherAttendanceDTO
    {
        public int AttendanceId { get; set; }

        public int TeacherId { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? ClockIn { get; set; }

        public TimeSpan? ClockOut { get; set; }
    }
}
