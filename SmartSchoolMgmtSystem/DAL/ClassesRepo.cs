 
using SmartSchool.Utilities;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;

namespace SmartSchool.DAL
{
    public class ClassesRepo: IClassesRepo
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _config;

        public ClassesRepo(MyDbContext context)
        {
            _context = context;
        }
        public List<ClassesDto> GetClasses(int id)
        {
            var result = (from user in _context.classEntity
                          where user.IsDeleted == false && user.CreatedBy==id
                          select new ClassesDto
                          {
                              ClassId = user.ClassId,
                              Grade = user.Grade,
                              Section = user.Section
                          }).ToList();

            return result;
        }


        public GenericResponse AddClass(ClassesDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            ClassesEntity entity = new ClassesEntity();
            int count = _context.classEntity.Where(a => a.Grade == obj.Grade && a.Section==obj.Section && a.CreatedBy==id &&a.IsDeleted == false).Count();
            int classId = _context.classEntity.OrderByDescending(a => a.ClassId).Select(a => a.ClassId).FirstOrDefault();

            try
            {
                if (count < 1)
                {
                    entity.ClassId = classId+1;
                    entity.Grade = obj.Grade;
                    entity.Section = obj.Section;
                    entity.IsDeleted = false;
                    entity.CreatedOn = DateTime.Now;
                    entity.CreatedBy = id;

                    _context.classEntity.Add(entity);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = entity.ClassId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Class Wthit this Section Alredy exist";
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to add Class" + ex;
            }
            return response;
        }

        public GenericResponse UpdateClass(ClassesDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.classEntity.Where(a => a.ClassId == obj.ClassId &&a.CreatedBy==id&& a.IsDeleted == false).FirstOrDefault();
            int count = _context.classEntity.Where(a => a.Grade == obj.Grade&& a.Section==obj.Section && a.CreatedBy == id && a.IsDeleted == false).Count();
            try
            {
                if (count == 0)
                {
                    result.ClassId = obj.ClassId;
                    result.Grade = obj.Grade;
                    result.Section = obj.Section;
                    result.IsDeleted = false;
                    result.CreatedOn = result.CreatedOn;
                    result.UpdatedOn = DateTime.Now;
                    result.UpdatedBy = id;

                    _context.classEntity.Update(result);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = result.ClassId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Class with this section Alredy exist";
                }

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to Update Class" + ex;
            }
            return response;
        }

        public GenericResponse DeleteClass(int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.classEntity.Where(a => a.ClassId == id && a.IsDeleted == false).FirstOrDefault();
            try
            {
                result.IsDeleted = true;
                _context.classEntity.Update(result);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Success";
                response.currentId = result.ClassId;

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Delete Class" + ex;
            }
            return response;
        }

    }
}
