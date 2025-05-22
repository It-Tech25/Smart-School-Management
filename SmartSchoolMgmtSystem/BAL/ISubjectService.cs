using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public interface ISubjectService
    {
        List<SubjectDto> GetSubject(int id);
        GenericResponse AddSubject(SubjectDto obj, int id);
        GenericResponse UpdateSubject(SubjectDto obj, int id);
        GenericResponse DeleteSubject(int id);
    }  
        
}
