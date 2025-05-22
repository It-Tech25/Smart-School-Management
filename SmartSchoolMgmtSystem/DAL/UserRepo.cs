 
using SmartSchool.Models.DTO;
using SmartSchool.Utilities;
//using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using SmartSchool.Models.Entity;

namespace SmartSchool.DAL
{
    public class UserRepo : IUserRepo
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _config;

        public UserRepo(MyDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


//Login check
        public LoginResponse LoginCheck(LoginRequest request)
        {
            LoginResponse lr = new LoginResponse();
            try
            {

                var u = _context.userEntity.Where(a => a.IsDeleted == false && a.IsActive == true && a.Email == request.Email).FirstOrDefault();

                if (u == null)
                {
                    lr.statusCode = 0;
                    lr.Message = "Please enter valid email";
                    return lr;
                }
                else
                {
                    var p = EncryptTool.Decrypt(u.PasswordHash);
                    if (request.Password == EncryptTool.Decrypt(u.PasswordHash))
                    {
                        lr.statusCode = 1;
                        lr.Message = "success";
                        lr.userTypeName = _context.userTypeEntites.Where(a => a.UserTypeId == u.UserTypeId).Select(b => b.UserTypeName).FirstOrDefault();
                        lr.userName = u.FullName;
                        lr.userId = u.UserId;
                       lr.profileUrl = u.ProfilePicture == null ? "dummy.png" : u.ProfilePicture;

                        // entry in lastlogin detials

                        lr.statusCode = 1;
                        lr.Message = "Login success";
                        return lr;
                    }
                    else
                    {
                        lr.statusCode = 0;
                        lr.Message = "Password incorrect";
                        return lr;
                    }
                }

            }
            catch (Exception ex)
            {


            }

            return lr;
        }

        //Count for SuperAdmin Dashboards
        public int TotalSubscriptions()
        {
            int count = _context.schools.Where(a => a.IsDeleted == false).Count();
            return count;
        }
        //User Type
        public List<UserTypeDTO> GetUserType(int id)
        {
            var result = (from user in _context.userTypeEntites
                          where user.IsDeleted == false && user.CreatedBy==id
                          select new UserTypeDTO
                          {
                              UserTypeId = user.UserTypeId,
                              UserTypeName = user.UserTypeName,
                              Description = user.Description
                          }).ToList();

            return result;
        }


        public GenericResponse AddUserType(UserTypeDTO obj , int id)
        {
            GenericResponse response = new GenericResponse();
            UserTypeEntites entity = new UserTypeEntites();
            int count = _context.userTypeEntites.Where(a => a.UserTypeName == obj.UserTypeName && a.CreatedBy==id && a.IsDeleted==false).Count();
            try
            {
                if (count ==0)
                {
                    entity.UserTypeName = obj.UserTypeName;
                    entity.Description = obj.Description;
                    entity.IsDeleted = false;
                    entity.IsActive = true;
                    entity.CreatedOn = DateTime.Now;
                    entity.CreatedBy = id;

                    _context.userTypeEntites.Add(entity);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = entity.UserTypeId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "User Type Alredy exist";
                }
            }
            catch(Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to add UserType" + ex;
            }
            return response;
        }

        public GenericResponse UpdateUserType(UserTypeDTO obj, int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.userTypeEntites.Where(a => a.UserTypeId == obj.UserTypeId && a.IsDeleted == false).FirstOrDefault();
            int count = _context.userTypeEntites.Where(a => a.UserTypeName == obj.UserTypeName).Count();
            try
            {
                if (count == 1)
                {
                    result.UserTypeId = obj.UserTypeId;
                    result.UserTypeName = obj.UserTypeName;
                    result.Description = obj.Description;
                    result.IsDeleted = false;
                    result.IsActive = true;
                    result.CreatedOn = result.CreatedOn;
                    result.UpdatedOn = DateTime.Now;
                    result.UpdatedBy = id;

                    _context.userTypeEntites.Update(result);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = result.UserTypeId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "User Type Alredy exist";
                }

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to Update UserType" + ex;
            }
            return response;
        }

        public GenericResponse DeleteUserType(int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.userTypeEntites.Where(a => a.UserTypeId == id && a.IsDeleted == false).FirstOrDefault();
            try
            {
                    result.IsDeleted = true;
                    result.IsActive = false;
                    _context.userTypeEntites.Update(result);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = result.UserTypeId;
              
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Delete UserType" + ex;
            }
            return response;
        }


        //UserMaster
        public List<UserDto> GetUser(int id)
        {
            var result = (from user in _context.userEntity
                          where user.IsDeleted == false && user.CreatedBy==id
                          select new UserDto
                          {
                              UserId = user.UserId,
                              FullName=user.FullName,
                              Email=user.Email,
                              ProfilePicture=user.ProfilePicture,
                              UserType = _context.userTypeEntites.Where(a=>a.UserTypeId== user.UserTypeId && a.IsDeleted==false).Select(a=>a.UserTypeName).FirstOrDefault()
                              
                          }).ToList();

            return result;
        }


        public GenericResponse AddUser(UserDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            UserEntity entity = new UserEntity();
            AddressEntity address = new AddressEntity();
            int AddressId = _context.addressEntity.OrderByDescending(a => a.AddressId).Select(a => a.AddressId).FirstOrDefault();
            int count = _context.userEntity.Where(a => a.FullName == obj.FullName && a.IsDeleted==false).Count();
            try
            {
                if (count < 1)
                {
                  
                    entity.FullName = obj.FullName;
                    entity.Email = obj.Email;
                    entity.ProfilePicture = obj.ProfilePicture;
                    entity.UserTypeId = _context.userTypeEntites.Where(a=>a.UserTypeName==obj.UserType && a.CreatedBy==id && a.IsDeleted==false).Select(a=>a.UserTypeId).FirstOrDefault();
                    entity.IsDeleted = false;
                    entity.IsActive = true;
                    entity.PasswordHash = EncryptTool.Encrypt(obj.PasswordHash);
                    entity.CreatedOn = DateTime.Now;
                    entity.CreatedBy = id;

                    _context.userEntity.Add(entity);
                    _context.SaveChanges();
                    address.AddressId = AddressId+1;
                    address.AddressLine = obj.AddressLine;
                    address.UserId = entity.UserId;
                    address.City = obj.City;
                    address.State = obj.State;
                    address.PinCode = obj.PinCode;
                    address.IsDeleted = false;
                    address.CreatedOn = DateTime.Now;
                    address.CreatedBy = id;
                    _context.addressEntity.Add(address);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = entity.UserId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "User Name Alredy exist";
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to add User" + ex;
            }
            return response;
        }

        public GenericResponse UpdateUser(UserDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.userEntity.Where(a => a.UserId == obj.UserId && a.IsDeleted == false).FirstOrDefault();
            int count = _context.userEntity.Where(a => a.FullName == obj.FullName).Count();
            try
            {
                if (count == 1)
                {
                    result.FullName = obj.FullName;
                    result.Email = obj.Email;
                    result.UserTypeId = _context.userTypeEntites.Where(a => a.UserTypeName == obj.UserType && a.IsDeleted == false).Select(a => a.UserTypeId).FirstOrDefault();
                    result.PasswordHash = EncryptTool.Encrypt(obj.PasswordHash);
                    result.IsDeleted = false;
                    result.IsActive = true;
                    result.CreatedOn = result.CreatedOn;
                    result.CreatedBy = result.CreatedBy;
                    result.UpdatedOn = DateTime.Now;
                    result.UpdatedBy = id;
                    if (obj.ProfilePicture == null)
                    {
                        result.ProfilePicture = result.ProfilePicture;
                    }
                    else
                    {
                        result.ProfilePicture = obj.ProfilePicture;
                    }
                        _context.userEntity.Update(result);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = result.UserId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "User Name Alredy exist";
                }

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Update User" + ex;
            }
            return response;
        }

        public GenericResponse DeleteUser(int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.userEntity.Where(a => a.UserId == id && a.IsDeleted == false).FirstOrDefault();
            try
            {
                result.IsDeleted = true;
                result.IsActive = false;
                _context.userEntity.Update(result);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Success";
                response.currentId = result.UserId;

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Delete User" + ex;
            }
            return response;
        }




        //Duration Entity

        public List<DurationDto> GetDuration()
        {
            var result = (from user in _context.duration
                          where user.IsDeleted == false
                          select new DurationDto
                          {
                              Durationid = user.Durationid,
                              DurationType = user.DurationType
                          }).ToList();

            return result;
        }


        public GenericResponse AddDuration(DurationDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            DurationEntity entity = new DurationEntity();
            int count = _context.duration.Where(a => a.DurationType == obj.DurationType && a.CreatedBy == id && a.IsDeleted == false).Count();
            try
            {
                if (count == 0)
                {
                    entity.DurationType = obj.DurationType;
                    entity.IsDeleted = false;
                    entity.IsActive = true;
                    entity.CreatedOn = DateTime.Now;
                    entity.CreatedBy = id;

                    _context.duration.Add(entity);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = entity.Durationid;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Duration Alredy exist";
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to add Duration" + ex;
            }
            return response;
        }

        public GenericResponse UpdateDuration(DurationDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.duration.Where(a => a.Durationid == obj.Durationid && a.IsDeleted == false).FirstOrDefault();
            int count = _context.duration.Where(a => a.DurationType == obj.DurationType && a.IsDeleted==false).Count();
            try
            {
                if (count < 1)
                {
                    result.Durationid = obj.Durationid;
                    result.DurationType = obj.DurationType;
                    result.IsDeleted = false;
                    result.IsActive = true;
                    result.CreatedOn = result.CreatedOn;
                    result.UpdatedOn = DateTime.Now;
                    result.UpdatedBy = id;

                    _context.duration.Update(result);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = result.Durationid;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Duration Alredy exist";
                }

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to Update Duration" + ex;
            }
            return response;
        }

        public GenericResponse DeleteDuration(int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.duration.Where(a => a.Durationid == id && a.IsDeleted == false).FirstOrDefault();
            try
            {
                result.IsDeleted = true;
                result.IsActive = false;
                _context.duration.Update(result);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Success";
                response.currentId = result.Durationid;

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Delete Duration" + ex;
            }
            return response;
        }


    }
}
