 
using SmartSchool.Utilities;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;

namespace SmartSchool.DAL
{
    
        public class ClassTimetableRepo : IClassTimetableRepo
        {
            private readonly MyDbContext _context;
            private readonly IConfiguration _config;

            public ClassTimetableRepo(MyDbContext context, IConfiguration config)
            {
                _context = context;
                _config = config;
            }


            public List<string> GetStates()
            {
                var result = _context.stateEntity.Select(a => a.StateName).ToList();
                return result;
            }

            public List<string> Getcity(string state)
            {
                int id = _context.stateEntity.Where(a => a.StateName == state).Select(a => a.StateId).FirstOrDefault();
                var result = _context.cityEntities.Where(a => a.StateId == id).Select(a => a.CityName).ToList();
                return result;
            }


        public List<ClassTimetableDto> GetClassTimetable()
        {
            var result = (from ClassTimetable in _context.classtimetableEntity
                          where ClassTimetable.IsDeleted == false
                          select new ClassTimetableDto
                          {
                              TimetableId = ClassTimetable.TimetableId,
                              Class = _context.classEntity.Where(a => a.ClassId == ClassTimetable.ClassId).Select(a => a.Grade + " - " + a.Section).FirstOrDefault(),
                              Subject = _context.subjectEntity.Where(a => a.SubjectId == ClassTimetable.SubjectId).Select(a => a.SubjectName).FirstOrDefault(),
                              FullName = _context.userEntity.Where(u => u.UserId == ClassTimetable.TeacherId).Select(u => u.FullName).FirstOrDefault(),
                              Room = ClassTimetable.Room,
                              DayOfWeek = ClassTimetable.DayOfWeek,
                              StartTime = ClassTimetable.StartTime,
                              EndTime = ClassTimetable.EndTime,
                          }).ToList();
            return result;  

        }
            public GenericResponse AddClassTimetable(ClassTimetableDto obj, int id)
            {
                GenericResponse response = new GenericResponse();
                ClassTimetableEntity entity = new ClassTimetableEntity();
                int ClassTimetableId= _context.classtimetableEntity.OrderByDescending(a => a.TimetableId).Select(a => a.TimetableId).FirstOrDefault();
                int count = _context.classtimetableEntity.Where(a => a.TimetableId == obj.TimetableId && a.CreatedBy == id && a.IsDeleted == false ).Count();
                try
                {
                    if (count < 1)
                    {
                        entity.TimetableId = ClassTimetableId+ 1;
                        entity.ClassId = obj.ClassId;
                        entity.SubjectId = obj.SubjectId;
                        entity.TeacherId = _context.userEntity.Where(u => u.FullName==obj.FullName && u.IsDeleted==false).Select(a=>a.UserId).FirstOrDefault();
                        entity.Room = obj.Room;
                        entity.DayOfWeek = obj.DayOfWeek;
                        entity.StartTime = obj.StartTime;
                        entity.EndTime = obj.EndTime;
                        entity.IsDeleted = false;
                        entity.CreatedOn = DateTime.Now;
                        entity.CreatedBy = id;
                        entity.UpdatedOn = DateTime.Now;
                        entity.UpdatedBy = id;

                        _context.classtimetableEntity.Add(entity);
                        _context.SaveChanges();
                        response.statuCode = 1;
                        response.message = "Success";
                        response.currentId = entity.TimetableId;
                    }
                    else
                    {
                        response.statuCode = 0;
                        response.message = "ClassTimetable Alredy exist";
                    }
                }
                catch (Exception ex)
                {
                    response.statuCode = 0;
                    response.message = "failed to add ClassTimetable" + ex;
                }
                return response;
            }

            public GenericResponse UpdateClassTimetable(ClassTimetableDto obj, int id)
            {
                GenericResponse response = new GenericResponse();
                var result = _context.classtimetableEntity.Where(a => a.TimetableId == obj.TimetableId && a.IsDeleted == false).FirstOrDefault();
                int count = _context.classtimetableEntity.Where(a => a.TimetableId == obj.TimetableId && a.CreatedBy==id && a.IsDeleted==false).Count();
                try
                {
                    if (count == 1)
                    {
                        result.TimetableId = obj.TimetableId;
                        result.ClassId = obj.ClassId;
                        result.SubjectId = obj.SubjectId;
                        result.TeacherId = _context.userEntity.Where(u => u.FullName == obj.FullName && u.IsDeleted == false).Select(a => a.UserId).FirstOrDefault();
                        result.Room = obj.Room;
                        result.DayOfWeek = obj.DayOfWeek;
                        result.StartTime = obj.StartTime;
                        result.EndTime = obj.EndTime;
                        result.IsDeleted = false;
                        result.IsDeleted = false;
                        result.CreatedOn = DateTime.Now;
                        result.CreatedBy = id;
                        result.UpdatedOn = DateTime.Now;
                        result.UpdatedBy = id;

                        _context.classtimetableEntity.Update(result);
                        _context.SaveChanges();
                        response.statuCode = 1;
                        response.message = "Success";
                        response.currentId = result.TimetableId;
                    }
                    else
                    {
                        response.statuCode = 0;
                        response.message = "ClassTimetable Alredy exist";
                    }

                }
                catch (Exception ex)
                {
                    response.statuCode = 0;
                    response.message = "failed to Update ClassTimetable" + ex;
                }
                return response;
            }

            public GenericResponse DeleteClassTimetable(int id)
            {
                GenericResponse response = new GenericResponse();
                var result = _context.classtimetableEntity.Where(a => a.TimetableId == id && a.IsDeleted == false).FirstOrDefault();
                try
                {
                    result.IsDeleted = true;
                    _context.classtimetableEntity.Update(result);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = result.TimetableId;

                }
                catch (Exception ex)
                {
                    response.statuCode = 0;
                    response.message = "Failed to Delete ClassTimetable" + ex;
                }
                return response;
            }

        }
    }

