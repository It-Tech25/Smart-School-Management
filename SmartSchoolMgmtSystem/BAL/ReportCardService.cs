

using SmartSchool.DAL;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;

namespace SmartSchool.BAL
{
    public class ReportCardService: IReportCardService
    {
        private readonly MyDbContext _context;
        private readonly IReportCardRepository _repository;

        public ReportCardService(MyDbContext context, IReportCardRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public void GenerateReportCardTable(int schoolId, int classId, List<string> subjects)
        {
            string tableName = $"ReportCard_School_{schoolId}_Class_{classId}";

            if (!_repository.TableExists(tableName))
            {
                _context.CreateDynamicTable(tableName, subjects);

                var masterReportCard = new MasterReportCard
                {
                    SchoolId = schoolId,
                    ClassId = classId,
                    TableName = tableName
                };

                _repository.RegisterTable(masterReportCard);
            }
        }

        public void AddStudentMarks(int schoolId, int classId, ReportCardDto reportCard)
        {
            string tableName = $"ReportCard_School_{schoolId}_Class_{classId}";
            _repository.AddReportCard(tableName, reportCard);
        }
        public List<ReportCardDto> GetReportCards(int schoolId, int classId)
        {
            string tableName = $"ReportCard_School_{schoolId}_Class_{classId}";
            return _repository.GetReportCards(tableName);
        }

        public void EditStudentMarks(int schoolId, int classId, ReportCardDto reportCard)
        {
            string tableName = $"ReportCard_School_{schoolId}_Class_{classId}";
            _repository.EditReportCard(tableName, reportCard.StudentId, reportCard.Marks);
        }

        public void DeleteStudentRecord(int schoolId, int classId, int studentId)
        {
            string tableName = $"ReportCard_School_{schoolId}_Class_{classId}";
            _repository.DeleteReportCard(tableName, studentId);
        }

    }
}
