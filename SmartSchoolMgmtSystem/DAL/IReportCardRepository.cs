using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;

namespace SmartSchool.DAL
{
    public interface IReportCardRepository
    {
        void AddReportCard(string tableName, ReportCardDto reportCard);
        bool TableExists(string tableName);
        void RegisterTable(MasterReportCard masterReportCard);
        List<ReportCardDto> GetReportCards(string tableName);
       void EditReportCard(string tableName, int studentId, Dictionary<string, int> marks);
        void DeleteReportCard(string tableName, int studentId);
    }
}
