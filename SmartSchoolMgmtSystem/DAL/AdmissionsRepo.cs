
using SmartSchool.Utilities;
using Microsoft.VisualBasic;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;

namespace SmartSchool.DAL
{
        public class AdmissionsRepo : IAdmissionsRepo
        {
            private readonly MyDbContext _context;
            private readonly IConfiguration _config;

            public AdmissionsRepo(MyDbContext context, IConfiguration config)
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


        public List<AdmissionsDto> GetAdmissions()
        {
            var result = (from Admission in _context.admissionsEntity
                          where (Admission.IsDeleted == false && Admission.IsAdded == false)
                          select new AdmissionsDto
                          {
                              AdmissionId = Admission.AdmissionId,
                              StudentName = Admission.StudentName,
                              School = _context.schools.Where(a => a.SchoolId == Admission.SchoolId).Select(a => a.Name).FirstOrDefault() ?? "N/A",
                              Class = _context.classEntity.Where(a => a.ClassId == Admission.ClassId).Select(a => a.Grade + " - " + a.Section).FirstOrDefault() ?? "N/A",
                              AdmissionFee = Admission.AdmissionFee,
                              TotalFee = Admission.TotalFee,
                              AcademicYear = Admission.AcademicYear,
                              Address = Admission.Address,
                              DOB = Admission.DOB,
                              Gender = Admission.Gender,
                              UserTypeId = Admission.UserTypeId,
                              AdharcardNo = Admission.AdharcardNo,
                              Photo = Admission.Photo
                          }).ToList();
            return result;
        }

        //public List<AdmissionsDto> GetAdmissions()
        //{
        //          var result = (from a in _context.admissionsEntity
        //          join s in _context.schools on a.SchoolId equals s.SchoolId into schoolJoin
        //          from s in schoolJoin.DefaultIfEmpty()
        //          join c in _context.classEntity on a.ClassId equals c.ClassId into classJoin
        //          from c in classJoin.DefaultIfEmpty()
        //          where a.IsDeleted == false && a.IsAdded == false
        //          select new AdmissionsDto
        //          {
        //              AdmissionId = a.AdmissionId,
        //              StudentName = a.StudentName ?? "N/A",
        //              School = s != null ? (s.Name ?? "N/A") : "N/A",
        //              Class = c != null ? ((c.Grade ?? "") + " - " + (c.Section ?? "")) : "N/A",
        //              AdmissionFee = a.AdmissionFee,
        //              TotalFee = a.TotalFee,
        //              AcademicYear = a.AcademicYear,
        //              UserTypeId = a.UserTypeId,
        //              Address = a.Address,
        //              DOB = a.DOB,
        //              Gender = a.Gender,
        //              AdharcardNo = a.AdharcardNo ?? "N/A",
        //              Photo = a.Photo ?? "no-photo.jpg"
        //          }).ToList();

        //         return result;
        //}




        public GenericResponse AddAdmission(AdmissionsDto obj, int UserId)
            {
                GenericResponse response = new GenericResponse();
                AdmissionsEntity entity = new AdmissionsEntity();
                int lastid = _context.admissionsEntity.OrderByDescending(a => a.AdmissionId).Select(a => a.AdmissionId).FirstOrDefault();
                int count = _context.admissionsEntity.Where(a => a.AdmissionId == obj.AdmissionId && a.CreatedBy == UserId && a.IsDeleted == false).Count();
            int? id = _context.userEntity.Where(s => s.UserId == UserId && s.IsDeleted == false).Select(a => a.CreatedBy).FirstOrDefault();
            int uType = _context.userTypeEntites.Where(a => a.UserTypeName == "Student"&& a.CreatedBy == id && a.IsDeleted == false).Select(a=>a.UserTypeId).FirstOrDefault();
            try
                {
                    if (count < 1)
                    {
                    entity.AdmissionId = lastid + 1;
                    entity.StudentName = obj.StudentName;
                    entity.SchoolId = _context.schools.Where(a => a.Name == obj.School && a.userid == id).Select(a => a.SchoolId).FirstOrDefault();
                    entity.ClassId = obj.ClassId;
                    entity.TotalFee = obj.TotalFee;
                    entity.AdmissionFee = obj.AdmissionFee;
                    entity.AcademicYear = DateTime.Now.Year.ToString();
                    entity.StudentName = obj.StudentName;
                    entity.UserTypeId = uType;
                    entity.AdharcardNo = obj.AdharcardNo;
                    entity.Photo = obj.Photo;
                    entity.Address = obj.Address;
                    entity.DOB = obj.DOB;
                    entity.Gender = obj.Gender;
                    entity.IsDeleted = false;
                    entity.IsAdded = false;
                    entity.CreatedOn = DateTime.Now;
                    entity.CreatedBy = UserId;

                    _context.admissionsEntity.Add(entity);
                    _context.SaveChanges();

                    response.statuCode = 1;
                    response.message = "Admission added successfully";
                    response.currentId = entity.AdmissionId;
                }
                    else
                    {
                        response.statuCode = 0;
                        response.message = "Admission Alredy exist";
                    }
                }
                catch (Exception ex)
                {
                    response.statuCode = 0;
                    response.message = "failed to add Admission" + ex;
                }
                return response;
            }

            public GenericResponse UpdateAdmission(AdmissionsDto obj, int userid)
            {
                GenericResponse response = new GenericResponse();
                var result = _context.admissionsEntity.Where(a => a.AdmissionId == obj.AdmissionId && a.IsDeleted == false).FirstOrDefault();
                int count = _context.admissionsEntity.Where(a => a.AdmissionId == obj.AdmissionId).Count();
         
            try
                {
                    if (count == 1)
                    {
                    result.AdmissionId = obj.AdmissionId;
                    result.StudentName = obj.StudentName;
                    result.SchoolId = _context.schools.Where(a => a.Name == obj.School && a.userid == userid).Select(a => a.SchoolId).FirstOrDefault();
                    result.ClassId = obj.ClassId;
                    result.TotalFee = obj.TotalFee; 
                    result.AdmissionFee = obj.AdmissionFee;
                    result.AcademicYear = obj.AcademicYear;
                    result.StudentName = obj.StudentName;
                    result.UserTypeId = obj.UserTypeId;
                    result.AdharcardNo = obj.AdharcardNo;
                    result.Photo = obj.Photo;
                    result.Address = obj.Address;
                    result.DOB = obj.DOB;
                    result.Gender = obj.Gender;
                    result.IsDeleted = false;
                    result.IsAdded = false;
                    result.UpdatedOn = DateTime.Now;
                    result.UpdatedBy = userid;

                    _context.admissionsEntity.Update(result);
                    _context.SaveChanges();

                    response.statuCode = 1;
                    response.message = "Admission updated successfully";
                    response.currentId = result.AdmissionId;
                }
                    else
                    {
                        response.statuCode = 0;
                        response.message = "Admission Alredy exist";
                    }

                }
                catch (Exception ex)
                {
                    response.statuCode = 0;
                    response.message = "failed to Update Admission" + ex;
                }
                return response;
            }

            public GenericResponse DeleteAdmission(int id)
            {
                GenericResponse response = new GenericResponse();
                var result = _context.admissionsEntity.Where(a => a.AdmissionId == id && a.IsDeleted == false).FirstOrDefault();
                try
                {
                result.IsDeleted = true;
                _context.admissionsEntity.Update(result);
                _context.SaveChanges();

                response.statuCode = 1;
                response.message = "Admission deleted successfully";
                response.currentId = result.AdmissionId;

            }
                catch (Exception ex)
                {
                    response.statuCode = 0;
                    response.message = "Failed to Delete SchoolAddress" + ex;
                }
                return response;
            }
        }
}
