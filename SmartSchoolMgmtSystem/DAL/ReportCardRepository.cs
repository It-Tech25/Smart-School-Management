

using Microsoft.EntityFrameworkCore;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;

namespace SmartSchool.DAL
{
    public class ReportCardRepository: IReportCardRepository
    {
        private readonly MyDbContext _context;

        public ReportCardRepository(MyDbContext context)
        {
            _context = context;
        }

        public void AddReportCard(string tableName, ReportCardDto reportCard)
        {
            var columns = string.Join(", ", reportCard.Marks.Keys);
            var values = string.Join(", ", reportCard.Marks.Values);

            string query = $@"
            INSERT INTO [{tableName}] (StudentId, {columns})
            VALUES ({reportCard.StudentId}, {values})
        ";

            _context.Database.ExecuteSqlRaw(query);
        }

        public bool TableExists(string tableName)
        {
            return _context.MasterReportCards.Any(t => t.TableName == tableName);
        }

        public void RegisterTable(MasterReportCard masterReportCard)
        {
            _context.MasterReportCards.Add(masterReportCard);
            _context.SaveChanges();
        }
        public List<ReportCardDto> GetReportCards(string tableName)
        {
            var result = new List<ReportCardDto>();

            string query = $"SELECT * FROM [{tableName}]";
            var connection = _context.Database.GetDbConnection();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var marks = new Dictionary<string, int>();
                        for (int i = 2; i < reader.FieldCount; i++)
                        {
                            marks.Add(reader.GetName(i), reader.IsDBNull(i) ? 0 : reader.GetInt32(i));
                        }

                        result.Add(new ReportCardDto
                        {
                            StudentId = reader.GetInt32(1),
                            Marks = marks
                        });
                    }
                }
            }
            return result;
        }

      
        public void EditReportCard(string tableName, int studentId, Dictionary<string, int> marks)
        {
            var updates = string.Join(", ", marks.Select(m => $"[{m.Key}] = {m.Value}"));
            string query = $"UPDATE [{tableName}] SET {updates} WHERE StudentId = {studentId}";
            _context.Database.ExecuteSqlRaw(query);
        }

        
        public void DeleteReportCard(string tableName, int studentId)
        {
            string query = $"DELETE FROM [{tableName}] WHERE StudentId = {studentId}";
            _context.Database.ExecuteSqlRaw(query);
        }
    }
}
