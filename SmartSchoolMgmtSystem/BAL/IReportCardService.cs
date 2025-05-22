using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public interface IReportCardService
    {
        void GenerateReportCardTable(int schoolId, int classId, List<string> subjects);
        void AddStudentMarks(int schoolId, int classId, ReportCardDto reportCard);
        List<ReportCardDto> GetReportCards(int schoolId, int classId);
        void EditStudentMarks(int schoolId, int classId, ReportCardDto reportCard);
        void DeleteStudentRecord(int schoolId, int classId, int studentId);
    }
}
