namespace SmartSchool.Models.DTO
{
    public class TeachersDto
    {
        public int TeacherId { get; set; }

        public int? UserId { get; set; }

        public int? SchoolId { get; set; }

        public int? DirectionId { get; set; }

     
        public int? ClassId { get; set; }

        public int? SubjectId { get; set; }
        public string FullName { get; set; }
        public string SubjectName { get; set; }
        public string School { get; set; }
        public string Class { get; set; }


    }
}
