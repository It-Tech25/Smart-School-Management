namespace SmartSchool.Models.DTO
{
    public class StudentAttendenceDto
    {
        public int AttendanceId { get; set; }
        public string StudentName { get; set; }

        public string ClassName { get; set; }

        public string SubjectName { get; set; }

        public int? StudentId { get; set; }

        public int? ClassId { get; set; }

        public int? SubjectId { get; set; }

        public DateTime? Date { get; set; }
        public string? Status { get; set; }

    }
}
