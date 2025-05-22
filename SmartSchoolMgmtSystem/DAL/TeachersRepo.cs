using SmartSchool.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System; 
using SmartSchool.Models.DTO;
using SmartSchool.Utilities; 

namespace SmartSchool.Repository
{
    public class TeachersRepo : ITeachersRepo
    {
        private readonly MyDbContext _context;

        public TeachersRepo(MyDbContext context)
        {
            _context = context;
        }
         
        public List<TeachersDto> GetAllAsync(int id)
        {
            var result = (from Teacher in _context.teacherEntity
                          where Teacher.IsDeleted == false
                          select new TeachersDto
                          {
                              TeacherId = Teacher.TeacherId,
                              FullName = _context.userEntity.Where(a=>a.UserId==Teacher.UserId).Select(a=>a.FullName).FirstOrDefault(),
                              School = _context.schools.Where(a => a.SchoolId == Teacher.SchoolId).Select(a => a.Name).FirstOrDefault(),
                              SubjectName = _context.subjectEntity.Where(a => a.SubjectId == Teacher.SubjectId).Select(a => a.SubjectName).FirstOrDefault(),
                              Class = _context.classEntity.Where(a => a.ClassId == Teacher.ClassId).Select(a => a.Grade + " - " + a.Section).FirstOrDefault(),

                          }).ToList();
            return result;
        }

        
        public  GenericResponse AddAsync(TeachersDto teacher, int id)
        {
            GenericResponse res = new GenericResponse();
            int teacherid = _context.teacherEntity.OrderByDescending(a => a.TeacherId).Select(a => a.TeacherId).FirstOrDefault();
            try
            {
                TeachersEntity teachers = new TeachersEntity();
                {
                    teachers.TeacherId = teacherid + 1;
                    teachers.UserId = _context.userEntity.Where(a => a.FullName == teacher.FullName).Select(a => a.UserId).FirstOrDefault();
                    teachers.SchoolId = _context.schools.Where(a => a.Name == teacher.School && a.userid==id).Select(a => a.SchoolId).FirstOrDefault();
                    teachers.ClassId = teacher.ClassId;
                    teachers.SubjectId = _context.subjectEntity.Where(a => a.SubjectName == teacher.SubjectName).Select(a => a.SubjectId).FirstOrDefault();
                    teachers.CreatedBy = id;
                    teachers.CreatedOn = DateTime.Now;
                    teachers.IsDeleted = false;

                }


                _context.teacherEntity.Add(teachers);
                _context.SaveChanges();
                res.statuCode = 1;
                res.message = "Sucessfully";
                return res;
            }
            catch (Exception ex)
            {
                res.statuCode = 0;
                res.message = "Failed to Add" + ex;
                return res;
            }
        }

                public GenericResponse UpdateAsync(TeachersDto teacher, int id)
        {
            GenericResponse res = new GenericResponse();
            var response = _context.teacherEntity.Where(a => a.TeacherId==teacher.TeacherId&&a.CreatedBy==id && a.IsDeleted==false).FirstOrDefault();
            try
            {
                
                    response.TeacherId = teacher.TeacherId;
                response.UserId = _context.userEntity.Where(a => a.FullName == teacher.FullName).Select(a => a.UserId).FirstOrDefault();
                response.SchoolId = _context.schools.Where(a => a.Name == teacher.School && a.userid == id).Select(a => a.SchoolId).FirstOrDefault();
                response.ClassId = teacher.ClassId;
                response.SubjectId = _context.subjectEntity.Where(a => a.SubjectName == teacher.SubjectName).Select(a => a.SubjectId).FirstOrDefault();
                response.CreatedBy = response.CreatedBy;
                response.CreatedOn = response.CreatedOn;
                response.IsDeleted = false;
                response.UpdatedOn = DateTime.Now;
                response.UpdatedBy = id;

               


                _context.teacherEntity.Update(response);
                _context.SaveChanges();
                res.statuCode = 1;
                res.message = "Sucessfully";
                return res;
            }
            catch (Exception ex)
            {
                res.statuCode = 0;
                res.message = "Failed to Update" + ex;
                return res;
            }
        }

        public GenericResponse DeleteAsync(int id, int logid)
        {
            GenericResponse res = new GenericResponse();
            var response = _context.teacherEntity.Where(a => a.TeacherId == id && a.CreatedBy == logid && a.IsDeleted == false).FirstOrDefault();
            try
            {

                response.IsDeleted = true;
                response.UpdatedOn = DateTime.Now;
                response.UpdatedBy = id;

                _context.teacherEntity.Update(response);
                _context.SaveChanges();
                res.statuCode = 1;
                res.message = "Sucessfully";
                return res;
            }
            catch (Exception ex)
            {
                res.statuCode = 0;
                res.message = "Failed to Delete" + ex;
                return res;
            }
        } 
        public async Task<IEnumerable<TeachersEntity>> GetAllAsync()
        {
            return await _context.teacherEntity.Where(t => !t.IsDeleted.HasValue || !t.IsDeleted.Value).ToListAsync();
        }

        public async Task<TeachersEntity> GetByIdAsync(int id)
        {
            return await _context.teacherEntity.FindAsync(id);
        }

        public async Task<TeachersEntity> AddAsync(TeachersEntity teacher)
        {
            teacher.CreatedOn = DateTime.UtcNow;
            _context.teacherEntity.Add(teacher);
            await _context.SaveChangesAsync();
            return teacher;
        }

        public async Task<TeachersEntity> UpdateAsync(TeachersEntity teacher)
        {
            teacher.UpdatedOn = DateTime.UtcNow;
            _context.teacherEntity.Update(teacher);
            await _context.SaveChangesAsync();
            return teacher;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var teacher = await _context.teacherEntity.FindAsync(id);
            if (teacher == null) return false;

            teacher.IsDeleted = true;
            teacher.UpdatedOn = DateTime.UtcNow;
            _context.teacherEntity.Update(teacher);
            await _context.SaveChangesAsync();
            return true; 
        }
    }
}
