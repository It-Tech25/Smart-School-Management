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

        // Get all schools
        public List<SchoolDto> GetSchool()
        {
            var result = (from school in _context.schools
                          where school.IsDeleted==false
                          select new SchoolDto
                          {
                              SchoolId = school.SchoolId,
                              Name = school.Name,
                              Code = school.Code,
                              IsActive = school.IsActive,
                              CreatedDate = school.CreatedDate,
                              ProfilePhoto1 = school.ProfilePhoto1,
                              ProfilePhoto2 = school.ProfilePhoto2,
                              ProfilePhoto3 = school.ProfilePhoto3,
                              IsDeleted = school.IsDeleted,
                              CreatedOn = school.CreatedOn,
                              CreatedBy = school.CreatedBy,
                              UpdatedBy = school.UpdatedBy,
                              UpdatedOn = school.UpdatedOn,
                              URL = school.URL
                          }).ToList();

            return result;
        }

        // Add a new school
        public GenericResponse AddSchool(SchoolDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            SchoolAddressEntity entityAddress = new SchoolAddressEntity();

            try
            {
                int existingCount = _context.schools.Count(a => a.Name == obj.Name && a.IsDeleted==false);
                int latestAddressId = _context.SchoolAddresses.OrderByDescending(a => a.AddressId).Select(a => a.AddressId).FirstOrDefault();
                int userId = _context.userEntity.Where(a => a.FullName == obj.UserName && a.IsDeleted==false).Select(a => a.UserId).FirstOrDefault();

                if (existingCount == 0)
                {
                    var entity = new SchoolEntity
                    {
                        Name = obj.Name,
                        Code = obj.Code,
                        userid = userId,
                        ProfilePhoto1 = obj.ProfilePhoto1,
                        ProfilePhoto2 = obj.ProfilePhoto2,
                        ProfilePhoto3 = obj.ProfilePhoto3,
                        URL = obj.URL,
                        Logo=obj.Logo,
                        IsDeleted = false,
                        IsActive = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = id
                    };

                    _context.schools.Add(entity);
                    _context.SaveChanges();

                    entityAddress.AddressId = latestAddressId + 1;
                    entityAddress.SchoolId = entity.SchoolId;
                    entityAddress.AddressType = obj.AddressType;
                    entityAddress.AddressLine = obj.AddressLine;
                    entityAddress.City = obj.City;
                    entityAddress.State = obj.State;
                    entityAddress.ZipCode = obj.ZipCode;
                    entityAddress.IsDeleted = false;
                    entityAddress.CreatedOn = DateTime.Now;
                    entityAddress.CreatedBy = id;

                    _context.SchoolAddresses.Add(entityAddress);
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

        // Update an existing school
        public GenericResponse UpdateSchool(SchoolDto obj, int id)
        {
            GenericResponse response = new GenericResponse();

            var school = _context.schools.FirstOrDefault(a => a.SchoolId == obj.SchoolId && a.IsDeleted==false);
            var address = _context.SchoolAddresses.FirstOrDefault(a => a.SchoolId == obj.SchoolId && a.IsDeleted==false);
            int nameCount = _context.schools.Count(a => a.Name == obj.Name);

            try
            {
                if (school != null && nameCount <= 1)
                {
                    school.Name = obj.Name;
                    school.CreatedDate = obj.CreatedDate;
                    school.Code = obj.Code;
                    school.ProfilePhoto1 = obj.ProfilePhoto1;
                    school.ProfilePhoto2 = obj.ProfilePhoto2;
                    school.ProfilePhoto3 = obj.ProfilePhoto3;
                    school.Logo = obj.Logo;
                    school.IsDeleted = false;
                    school.IsActive = true;
                    school.UpdatedOn = DateTime.Now;
                    school.UpdatedBy = id;

                    _context.schools.Update(school);
                    _context.SaveChanges();

                    if (address != null)
                    {
                        address.AddressType = obj.AddressType;
                        address.AddressLine = obj.AddressLine;
                        address.City = obj.City;
                        address.State = obj.State;
                        address.ZipCode = obj.ZipCode;
                        address.IsDeleted = false;
                        address.UpdatedOn = DateTime.Now;
                        address.UpdatedBy = id;

                        _context.SchoolAddresses.Update(address);
                        _context.SaveChanges();
                    }

                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = school.SchoolId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "School already exists or invalid data";
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to update school. Error: " + ex.Message;
            }

            return response;
        }

        // Soft delete school
        public GenericResponse DeleteSchool(int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.schools.FirstOrDefault(a => a.SchoolId == id && a.IsDeleted==false);

            try
            {
                if (result != null)
                {
                    result.IsDeleted = true;
                    result.IsActive = false;

                    _context.schools.Update(result);
                    _context.SaveChanges();

                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = result.SchoolId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "School not found";
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to delete school. Error: " + ex.Message;
            }

            return response;
        }
    }
}
