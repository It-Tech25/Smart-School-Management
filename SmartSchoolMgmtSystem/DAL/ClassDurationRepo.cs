using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;
using SmartSchool.Utilities;

namespace SmartSchool.DAL
{
    public class ClassDurationRepo: IClassDurationRepo
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _config;

        public ClassDurationRepo(MyDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public List<ClassDurationDto> Classtiming(int id)
        {
            var result = (from user in _context.classDuration
                          where user.IsDeleted == false && user.CreatedBy == id
                          select new ClassDurationDto
                          {
                              ClassDurationId = user.ClassDurationId,
                              Period = user.Period,
                              StartTime = user.StartTime,
                              EndTime = user.EndTime,
                              Duration=user.Duration
                          }).ToList();

            return result;
        }

        public GenericResponse AddClasstiming(ClassDurationDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            ClassDurationEntity entity = new ClassDurationEntity();
            int count = _context.classDuration.Where(a => a.StartTime == obj.StartTime && a.CreatedBy==id && a.IsDeleted == false).Count();
            try
            {
                if (count < 1)
                {
                    entity.Period = obj.Period;
                    entity.StartTime = obj.StartTime;
                    entity.EndTime = obj.EndTime;
                    entity.Duration = obj.Duration;
                    entity.IsDeleted = false;
                    entity.IsActive = true;
                    entity.CreatedOn = DateTime.Now;
                    entity.CreatedBy = id;

                    _context.classDuration.Add(entity);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = entity.ClassDurationId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = " ClassDuration Alredy exist";
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to add ClassDuration" + ex;
            }
            return response;
        }

        public GenericResponse UpdateClasstiming(ClassDurationDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.classDuration.Where(a => a.ClassDurationId == obj.ClassDurationId && a.CreatedBy == id && a.IsDeleted == false).FirstOrDefault();
            int count = _context.classDuration.Where(a => a.StartTime == obj.StartTime && a.CreatedBy == id && a.IsDeleted == false).Count();
            try
            {
                if (count == 1)
                {
                    result.Period = obj.Period;
                    result.StartTime = obj.StartTime;
                    result.EndTime = obj.EndTime;
                    result.Duration = obj.Duration;
                    result.IsDeleted = false;
                    result.IsActive = true;
                    result.CreatedOn = result.CreatedOn;
                    result.UpdatedOn = DateTime.Now;
                    result.UpdatedBy = id;

                    _context.classDuration.Update(result);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = result.ClassDurationId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "ClassDuration Alredy exist";
                }

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to Update ClassDuration" + ex;
            }
            return response;


        }
        public GenericResponse DeleteClassDuration(int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.classDuration.Where(a => a.ClassDurationId == id && a.IsDeleted == false).FirstOrDefault();
            try
            {
                result.IsDeleted = true;
                result.IsActive = false;
                _context.classDuration.Update(result);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Success";
                response.currentId = result.ClassDurationId;

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Delete ClassDuration" + ex;
            }
            return response;
        }

    }
}
