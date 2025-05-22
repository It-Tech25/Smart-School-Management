using SmartSchool.DAL;
using SmartSchool.Models.Entity;
using SmartSchool.Utilities;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace SmartSchool.DAL
{
    public class StudentRepo :IStudentRepo
    {
      
            private readonly MyDbContext _context;
            private readonly IConfiguration _config;

            public StudentRepo(MyDbContext context, IConfiguration config)
            {
                _context = context;
                _config = config;
            }
        public List<ClassesDto> GetClasses(int? id)
        {
            var result = _context.classEntity
                .Where(c => c.IsDeleted == false && c.CreatedBy == id)
                .Select(c => new ClassesDto
                {
                    ClassId = c.ClassId,
                    Grade = c.Grade,
                    Section = c.Section
                })
                .ToList();

            return result;
        }
        //public List<StudentDTO> GetStudent(int id)
        //{
        //    var result = (from student in _context.studentEntity.Include(s => s.StudentId) // Include FamilyDetails
        //                  where (student.IsDeleted == false && student.CreatedBy == id)
        //                  select new StudentDTO
        //                  {
        //                      StudentId = student.StudentId,
        //                      StudentName = student.StudentName,
        //                      ClassID=student.ClassID,
        //                     //Class= _context.classEntity.Where(a => a.ClassId == student.ClassID && a.IsDeleted == false).Select(a => a.Grade).FirstOrDefault(),
        //                  Class = _context.classEntity.Where(a => a.ClassId == student.ClassID && a.IsDeleted == false).Select(a => (a.Grade ?? "NoGrade") + " - " + (a.Section ?? "NoSection")).FirstOrDefault() ?? "NoGrade - NoSection",
        //                      DOB = student.DOB,
        //                      TotalFee = student.TotalFee,
        //                      PaiedFee = student.PaiedFee,
        //                      SchoolId = student.SchoolId,
        //                      UserTypeId = student.UserTypeId,
        //                      AdharcardNo = student.AdharcardNo,
        //                      Address=student.Address,
        //                      Photo = student.Photo ?? string.Empty,
        //                      Gender = student.Gender ?? "Unknown",
        //                      //FamilyName = s.FamilyDetails.FirstOrDefault().Name,
        //                      //Relation = s.FamilyDetails.FirstOrDefault().Relation,
        //                      //ContactNumber = s.FamilyDetails.FirstOrDefault().ContactNumber
        //                  }).ToList();

        //    return result;
        //}
        public List<StudentDTO> GetStudent(int id)
        {
            var students = _context.studentEntity
                .Where(s => s.IsDeleted == false && s.CreatedBy == id)
                .Select(s => new StudentDTO
                {
                    StudentId = s.StudentId,
                    StudentName = s.StudentName,
                    Class = _context.classEntity
                                .Where(a => a.ClassId == s.ClassID && a.IsDeleted == false)
                                .Select(a => (a.Grade ?? "NoGrade") + " - " + (a.Section ?? "NoSection"))
                                .FirstOrDefault() ?? "NoGrade - NoSection",
                    Address = s.Address,
                    DOB = s.DOB,
                    Gender = s.Gender,
                    AdharcardNo = s.AdharcardNo,
                    Photo = s.Photo,
                    // Family Details
                    Name = s.Family.FirstOrDefault() != null ? s.Family.FirstOrDefault().Name : "N/A",
                    Relation = s.Family.FirstOrDefault() != null ? s.Family.FirstOrDefault().Relation : "N/A",
                    ContactNumber = s.Family.FirstOrDefault() != null ? s.Family.FirstOrDefault().ContactNumber : "N/A"
                }).ToList();

            return students;
        }


        public GenericResponse AddStudent(StudentDTO obj, int id)
        {
            GenericResponse response = new GenericResponse();
            StudentEntity entity= new StudentEntity();
            StudentFamily family = new StudentFamily();
           

           // int uid = _context.userEntity.Where(a => a.AddressId == obj.AddressId && a.IsDeleted == false).Select(a => a.UserId).FirstOrDefault();
            int sid = _context.schools.Where(a => a.SchoolId == obj.SchoolId && a.IsDeleted == false).Select(a => a.SchoolId).FirstOrDefault();
            int uid = _context.userTypeEntites.Where(a => a.UserTypeId == obj.UserTypeId && a.IsDeleted == false).Select(a => a.UserTypeId).FirstOrDefault();
            int cid = _context.classEntity.Where(a => a.ClassId == obj.ClassID && a.IsDeleted == false).Select(a => a.ClassId).FirstOrDefault();
            int count = _context.studentEntity.Where(a => a.StudentId == obj.StudentId).Count();
            int Studentid = _context.studentEntity.OrderByDescending(a => a.StudentId).Select(a => a.StudentId).FirstOrDefault();
            try
            {
                if (count < 1)
                {
                   entity.StudentId = Studentid+1;
                    entity.StudentName = obj.StudentName;
                    // entity.ClassID = Convert.ToInt32(obj.Class);
                    entity.ClassID = Convert.ToInt32(obj.Class);
                    entity.Address= obj.Address;
                    entity.SchoolId = 6;
                    entity.DOB= obj.DOB;
                    entity.UserTypeId = uid;
                    entity.AdharcardNo = obj.AdharcardNo;
                    entity.Photo = obj.Photo;
                    entity.Gender = obj.Gender;
                    entity.TotalFee = obj.TotalFee;
                    entity.PaiedFee = obj.PaiedFee;
                    entity.IsDeleted = false;
                    entity.CreatedOn = DateTime.Now;
                    entity.CreatedBy =id;
                    entity.IsAdded = false;
                    entity.UpdatedOn= DateTime.Now;
                    entity.UpdatedBy =id;
                    

                    _context.studentEntity.Add(entity);
                    _context.SaveChanges();
                    int familyid = _context.studentFamilyEntity.OrderByDescending(a => a.FamilyId).Select(a => a.FamilyId).FirstOrDefault();
                    if (obj.Name != null)
                    {
                        family.FamilyId = familyid+1;
                        family.Name = obj.Name;
                        family.StudentId = entity.StudentId;
                        family.ContactNumber = obj.ContactNumber;
                        family.Relation = obj.Relation;
                        family.IsDeleted = false;
                        family.CreatedOn = DateTime.Now;
                        family.CreatedBy = id;
                        _context.studentFamilyEntity.Add(family);
                        _context.SaveChanges();
                    }
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = entity.StudentId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "student id Alredy exist";
                }

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to add student" + ex;
            }
            return response;
        }

        public GenericResponse UpdateStudent(StudentDTO obj, int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                // Fetch the Student record from the database
                var result = _context.studentEntity
                    .Where(a => a.StudentId == obj.StudentId && a.IsDeleted == false)
                    .FirstOrDefault();

                int uid = _context.userTypeEntites
                    .Where(a => a.UserTypeName == "Student" && a.CreatedBy==id && a.IsDeleted == false).Select(a=>a.UserTypeId)
                    .FirstOrDefault();

                // If student not found, return error
                if (result == null)
                {
                    response.statuCode = 0;
                    response.message = "Student not found.";
                    return response;
                }

                // Update StudentEntity fields
                result.StudentName = obj.StudentName;
                result.UserTypeId = uid;
                result.AdharcardNo = obj.AdharcardNo;
                result.Photo = obj.Photo;
                result.ClassID = Convert.ToInt32(obj.Class);
                result.Address = obj.Address;
                result.DOB = obj.DOB;
                result.Gender = obj.Gender;
                result.TotalFee = obj.TotalFee;
                result.PaiedFee = obj.PaiedFee;
                result.UpdatedOn = DateTime.Now;
                result.UpdatedBy = id;

                // Update the student entity in the database
                _context.studentEntity.Update(result);
                _context.SaveChanges();

                // Update or Add Family Details if it exists
                var family = _context.studentFamilyEntity
                    .Where(f => f.StudentId == obj.StudentId && f.IsDeleted == false)
                    .FirstOrDefault();

                if (family != null)
                {
                    // Update Family Information
                    family.Name = obj.Name;
                    family.ContactNumber = obj.ContactNumber;
                    family.Relation = obj.Relation;
                    family.UpdatedOn = DateTime.Now;
                    family.UpdatedBy = id;

                    _context.studentFamilyEntity.Update(family);
                }
                else if (obj.Name != null)
                {
                    // If family does not exist, add a new one
                    int familyid = _context.studentFamilyEntity.OrderByDescending(a => a.FamilyId).Select(a => a.FamilyId).FirstOrDefault();
                    StudentFamily newFamily = new StudentFamily
                    {
                        FamilyId = familyid + 1,
                        StudentId = obj.StudentId,
                        Name = obj.Name,
                        ContactNumber = obj.ContactNumber,
                        Relation = obj.Relation,
                        IsDeleted = false,
                        CreatedOn = DateTime.Now,
                        CreatedBy = id
                    };
                    _context.studentFamilyEntity.Add(newFamily);
                }

                // Save changes
                _context.SaveChanges();

                // Return Success Response
                response.statuCode = 1;
                response.message = "Student updated successfully.";
                response.currentId = result.StudentId;
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to update Student: " + ex.Message;
            }

            return response;
        }
        public GenericResponse DeleteStudent(int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.studentEntity.Where(a => a.StudentId == id && a.IsDeleted == false).FirstOrDefault();
            try
            {
                result.IsDeleted = true;
                _context.studentEntity.Update(result);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Success";
                response.currentId = result.StudentId;

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Delete UserType" + ex;
            }
            return response;
        }
    }
}
