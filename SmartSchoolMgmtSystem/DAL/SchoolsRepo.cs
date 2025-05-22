using SmartSchool.DAL; 
using SmartSchool.Utilities;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;

namespace SmartSchool.DAL
{
    public class SchoolsRepo : ISchoolRepo
	{
       
            private readonly MyDbContext _context;
            private readonly IConfiguration _config;

            public SchoolsRepo(MyDbContext context, IConfiguration config)
            {
                _context = context;
                _config = config;
            }


         
            //User Type
            public List<SchoolDto> GetSchool()
            {
                var result = (from School in _context.schools
                              where School.IsDeleted == false
                              select new SchoolDto
                              {
                                  SchoolId = School.SchoolId,
                                  Name = School.Name,
                                  Code = School.Code,
                                  IsActive = School.IsActive,
                                  CreatedDate = School.CreatedDate,
                                  ProfilePhoto1=School.ProfilePhoto1,
                                  ProfilePhoto2=School.ProfilePhoto2,
                                  ProfilePhoto3=School.ProfilePhoto3,
                                  IsDeleted = School.IsDeleted,
                                  CreatedOn = School.CreatedOn,
                                  CreatedBy = School.CreatedBy,
                                  UpdatedBy = School.UpdatedBy,
                                  UpdatedOn = School.UpdatedOn,
                              }).ToList();

                return result;
            }


            public GenericResponse AddSchool(SchoolDto obj, int id)
            {
            GenericResponse response = new GenericResponse();
            SchoolAddressEntity entitys = new SchoolAddressEntity();
            try
            {
                int count = _context.schools.Count(a => a.Name == obj.Name && a.IsDeleted == false);
                int AddressId = _context.SchoolAddresses.OrderByDescending(a => a.AddressId).Select(a => a.AddressId).FirstOrDefault();
                int uId = _context.userEntity.Where(a => a.FullName==obj.UserName && a.IsDeleted==false).Select(a => a.UserId).FirstOrDefault();
                if (count == 0)
                {
                    var entity = new SchoolEntity
                    {
                        Name = obj.Name,
                        Code = obj.Code,
                        ProfilePhoto1=obj.ProfilePhoto1,
                        ProfilePhoto2=obj.ProfilePhoto2,
                        ProfilePhoto3=obj.ProfilePhoto3,
                        IsDeleted = false,
                        IsActive = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = id
                    };

                    _context.schools.Add(entity);
                    _context.SaveChanges();
                    entitys.AddressId = AddressId + 1;
                    entitys.SchoolId = entity.SchoolId;
                    entitys.AddressType = obj.AddressType;
                    entitys.AddressLine = obj.AddressLine;
                    entitys.City = obj.City;
                    entitys.State = obj.State;
                    entitys.ZipCode = obj.ZipCode;
                   
                    entitys.IsDeleted = false;
                    entitys.CreatedOn = DateTime.Now;
                    entitys.CreatedBy = id;

                    _context.SchoolAddresses.Add(entitys);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = entity.SchoolId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "School already exists";
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to add school. Error: " + ex.Message;
            }

            return response;

        }

        public GenericResponse UpdateSchool(SchoolDto obj, int id)
            {
                GenericResponse response = new GenericResponse();

                var result = _context.schools.Where(a => a.SchoolId == obj.SchoolId && a.IsDeleted == false).FirstOrDefault();
            var Address = _context.SchoolAddresses.Where(a => a.SchoolId == obj.SchoolId && a.IsDeleted == false).FirstOrDefault();
            int count = _context.schools.Where(a => a.Name == obj.Name).Count();
                try
                {
                    if (count == 1)
                    {
                        result.SchoolId = obj.SchoolId;
                        result.Name = obj.Name;
                        result.CreatedDate = obj.CreatedDate;

                    result.ProfilePhoto1 = obj.ProfilePhoto1;
                    result.ProfilePhoto2 = obj.ProfilePhoto2;
                    result.ProfilePhoto3 = obj.ProfilePhoto3;
                        result.Code = obj.Code;
                        result.IsDeleted = false;
                        result.IsActive = true;
                        result.CreatedOn = result.CreatedOn;
                        result.UpdatedOn = DateTime.Now;
                        result.UpdatedBy = id;

                        _context.schools.Update(result);
                        _context.SaveChanges();
                   
                 
                    Address.AddressId = Address.AddressId;
                    Address.SchoolId = obj.SchoolId;
                    Address.AddressType = obj.AddressType;
                    Address.AddressLine = obj.AddressLine;
                    Address.City = obj.City;
                    Address.State = obj.State;
                    Address.ZipCode = obj.ZipCode;
                     Address.IsDeleted = false;
                    Address.CreatedOn = result.CreatedOn;
                    Address.CreatedBy = result.CreatedBy;
                    Address.UpdatedOn = DateTime.Now;
                    Address.UpdatedBy = id;

                            _context.SchoolAddresses.Update(Address);
                            _context.SaveChanges();

                            response.statuCode = 1;
                        response.message = "Success";
                        response.currentId = result.SchoolId;
                    }
                    else
                    {
                        response.statuCode = 0;
                        response.message = "School Alredy exist";
                    }

                }
                catch (Exception ex)
                {
                    response.statuCode = 0;
                    response.message = "failed to Update School" + ex;
                }
                return response;
            }

            public GenericResponse DeleteSchool(int id)
            {
                GenericResponse response = new GenericResponse();
                var result = _context.schools.Where(a => a.SchoolId == id && a.IsDeleted == false).FirstOrDefault();
                try
                {
                    result.IsDeleted = true;
                    result.IsActive = false;
                    _context.schools.Update(result);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = result.SchoolId;

                }
                catch (Exception ex)
                {
                    response.statuCode = 0;
                    response.message = "Failed to Delete School" + ex;
                }
                return response;
            }

        }

    
}
