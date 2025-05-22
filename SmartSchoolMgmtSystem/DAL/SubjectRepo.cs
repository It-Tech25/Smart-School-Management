
using SmartSchool.Utilities;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;

namespace SmartSchool.DAL
{
    public class SubjectRepo : ISubjectRepo
    {

        private readonly MyDbContext _context;
        private readonly IConfiguration _config;

        public SubjectRepo(MyDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public List<SubjectDto> GetSubject(int id)
        {
            var result = (from user in _context.subjectEntity
                          where user.IsDeleted == false && user.CreatedBy == id
                          select new SubjectDto
                          {
                              SubjectId = user.SubjectId,
                              SubjectName = user.SubjectName,
                              Description = user.Description
                          }).ToList();

            return result;
        }

        public GenericResponse AddSubject(SubjectDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            SubjectEntity entity = new SubjectEntity();
            int count = _context.subjectEntity.Where(a => a.SubjectName == obj.SubjectName && a.IsDeleted == false).Count();
            try
            {
                if (count < 1)
                {
                    entity.SubjectName = obj.SubjectName;
                    entity.Description = obj.Description;
                    entity.IsDeleted = false;
                    entity.IsActive = true;
                    entity.CreatedOn = DateTime.Now;
                    entity.CreatedBy = id;

                    _context.subjectEntity.Add(entity);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = entity.SubjectId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = " Subject Name Alredy exist";
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to add Subject Name" + ex;
            }
            return response;
        }

        public GenericResponse UpdateSubject(SubjectDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.subjectEntity.Where(a => a.SubjectId == obj.SubjectId &&a.CreatedBy==id && a.IsDeleted == false).FirstOrDefault();
            int count = _context.subjectEntity.Where(a => a.SubjectName == obj.SubjectName && a.CreatedBy==id && a.IsDeleted==false).Count();
            try
            {
                if (count == 1)
                {
                    result.SubjectId = obj.SubjectId;
                    result.SubjectName = obj.SubjectName;
                    result.Description = obj.Description;
                    result.IsDeleted = false;
                    result.IsActive = true;
                    result.CreatedOn = result.CreatedOn;
                    result.UpdatedOn = DateTime.Now;
                    result.UpdatedBy = id;

                    _context.subjectEntity.Update(result);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = result.SubjectId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Subject Alredy exist";
                }

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to Update Subject" + ex;
            }
            return response;


        }
        public GenericResponse DeleteSubject(int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.subjectEntity.Where(a => a.SubjectId == id && a.IsDeleted == false).FirstOrDefault();
            try
            {
                result.IsDeleted = true;
                result.IsActive = false;
                _context.subjectEntity.Update(result);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Success";
                response.currentId = result.SubjectId;

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Delete Subject" + ex;
            }
            return response;
        }

    }
}
