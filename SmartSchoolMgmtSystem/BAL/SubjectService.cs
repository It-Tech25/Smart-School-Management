using SmartSchool.Utilities;
using SmartSchool.DAL;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepo _subjectRepo;
        public SubjectService(ISubjectRepo subjectRepo)
        {
            _subjectRepo = subjectRepo;
        }
        public List<SubjectDto> GetSubject(int id)
        {
            return _subjectRepo.GetSubject(id);
        }

        public GenericResponse AddSubject(SubjectDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response = _subjectRepo.AddSubject(obj, id);
            return response;
        }
        public GenericResponse UpdateSubject(SubjectDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response = _subjectRepo.UpdateSubject(obj, id);
            return response;
        }
        public GenericResponse DeleteSubject(int id)
        {
            GenericResponse response = new GenericResponse();
            response=_subjectRepo.DeleteSubject(id);
            return response;
        }
    }

}
