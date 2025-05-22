using SmartSchool.DAL; 
using SmartSchool.Utilities;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;

namespace SmartSchool.DAL
{
        public class SchoolAdressRepo : ISchoolAddressRepo
        {
            private readonly MyDbContext _context;
            private readonly IConfiguration _config;

            public SchoolAdressRepo(MyDbContext context, IConfiguration config)
            {
                _context = context;
                _config = config;
            }


        public List<string> GetStates()
        {
            var result = _context.stateEntity.Select(a=>a.StateName).ToList();
            return result;
        }

        public List<string> Getcity(string state)
        {
            int id = _context.stateEntity.Where(a => a.StateName == state).Select(a => a.StateId).FirstOrDefault();
            var result = _context.cityEntities.Where(a=>a.StateId==id).Select(a => a.CityName).ToList();
            return result;
        }


        public List<SchoolAddressDto> GetSchoolAddress()
            {
                var result = (from SchoolAddress in _context.SchoolAddresses
                              where SchoolAddress.IsDeleted == false
                              select new SchoolAddressDto
                              {
                                  AddressId = SchoolAddress.AddressId,
                                  SchoolId = SchoolAddress.SchoolId,
                                  AddressType = SchoolAddress.AddressType,
                                  AddressLine = SchoolAddress.AddressLine,
                                  City = SchoolAddress.City,
                                  State = SchoolAddress.State,
                                  ZipCode = SchoolAddress.ZipCode
                              }).ToList();
                return result;
            }
            public GenericResponse AddSchoolAddress(SchoolAddressDto obj, int id)
            {
                GenericResponse response = new GenericResponse();
                SchoolAddressEntity entity = new SchoolAddressEntity();
            int AddressId = _context.SchoolAddresses.OrderByDescending(a => a.AddressId).Select(a => a.AddressId).FirstOrDefault();
            int count = _context.SchoolAddresses.Where(a => a.SchoolId == obj.SchoolId &&a.CreatedBy==id && a.IsDeleted == false).Count();
                try
                {
                    if (count < 1)
                    {
                       entity.AddressId = AddressId+1;
                        entity.SchoolId = obj.SchoolId;
                        entity.AddressType = obj.AddressType;
                        entity.AddressLine = obj.AddressLine;
                        entity.City = obj.City;
                        entity.State = obj.State;
                        entity.ZipCode = obj.ZipCode;
                        entity.IsDeleted = false;
                        entity.CreatedOn = DateTime.Now;
                        entity.CreatedBy = id;

                        _context.SchoolAddresses.Add(entity);
                        _context.SaveChanges();
                        response.statuCode = 1;
                        response.message = "Success";
                        response.currentId = entity.AddressId;
                    }
                    else
                    {
                        response.statuCode = 0;
                        response.message = "SchoolAddress Alredy exist";
                    }
                }
                catch (Exception ex)
                {
                    response.statuCode = 0;
                    response.message = "failed to add SchoolAddress" + ex;
                }
                return response;
            }

            public GenericResponse UpdateSchoolAddress(SchoolAddressDto obj, int id)
            {
                GenericResponse response = new GenericResponse();
                var result = _context.SchoolAddresses.Where(a => a.AddressId == obj.AddressId && a.IsDeleted == false).FirstOrDefault();
                int count = _context.SchoolAddresses.Where(a => a.SchoolId == obj.SchoolId).Count();
                try
                {
                    if (count == 1)
                    {
                        result.AddressId = obj.AddressId;
                        result.SchoolId = obj.SchoolId;
                        result.AddressType = obj.AddressType;
                        result.AddressLine = obj.AddressLine;
                        result.City = obj.City;
                        result.State = obj.State;
                        result.ZipCode = obj.ZipCode;
                        result.IsDeleted = false;
                        result.CreatedOn = result.CreatedOn;
                        result.CreatedBy = result.CreatedBy;
                        result.UpdatedOn = DateTime.Now;
                        result.UpdatedBy = id;

                        _context.SchoolAddresses.Update(result);
                        _context.SaveChanges();
                        response.statuCode = 1;
                        response.message = "Success";
                        response.currentId = result.AddressId;
                    }
                    else
                    {
                        response.statuCode = 0;
                        response.message = "schoolAddress Alredy exist";
                    }

                }
                catch (Exception ex)
                {
                    response.statuCode = 0;
                    response.message = "failed to Update SchoolAddress" + ex;
                }
                return response;
            }

            public GenericResponse DeleteSchoolAddress(int id)
            {
                GenericResponse response = new GenericResponse();
                var result = _context.SchoolAddresses.Where(a => a.AddressId == id && a.IsDeleted == false).FirstOrDefault();
                try
                {
                    result.IsDeleted = true;
                    _context.SchoolAddresses.Update(result);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = result.AddressId;

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
