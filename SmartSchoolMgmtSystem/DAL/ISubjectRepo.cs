using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.DAL
{
    public interface ISubjectRepo
    {
        List<SubjectDto> GetSubject(int id);
        GenericResponse AddSubject(SubjectDto obj, int id);
        GenericResponse UpdateSubject(SubjectDto obj, int id);
        GenericResponse DeleteSubject(int id);
    }
}
