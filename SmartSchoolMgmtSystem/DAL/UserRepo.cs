using Humanizer.Localisation;
//using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;
using SmartSchool.Utilities;
using System.Text.RegularExpressions;

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

                        if (lr.userTypeName == "Super Admin")
                        {
                            lr.schoolLogo = "/resources/assets/images/SSMSLogo.png";
                        }
                        else if (lr.userTypeName == "School Admin")
                        {
                            lr.schoolLogo = _context.schools
                                .Where(a => a.userid == u.UserId && a.IsDeleted == false)
                                .Select(a => a.Logo)
                                .FirstOrDefault();
                        }
                        else
                        {
                            lr.schoolLogo = _context.schools
                                .Where(a => a.userid == u.CreatedBy && a.IsDeleted == false)
                                .Select(a => a.Logo)
                                .FirstOrDefault();
                        }

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
                lr.statusCode = 0;
                lr.Message = "An error occurred during login: " + ex.Message;
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
                          where user.IsDeleted == false && user.CreatedBy == id
                          select new UserTypeDTO
                          {
                              UserTypeId = user.UserTypeId,
                              UserTypeName = user.UserTypeName,
                              Description = user.Description
                          }).ToList();

            return result;
        }

        public GenericResponse AddUserType(UserTypeDTO obj, int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                UserTypeEntites entity = new UserTypeEntites();
                int count = _context.userTypeEntites.Where(a => a.UserTypeName == obj.UserTypeName && a.CreatedBy == id && a.IsDeleted == false).Count();

                if (count == 0)
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
                    response.message = "User Type Already exists";
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to add UserType: " + ex.Message;
            }
            return response;
        }

        public GenericResponse UpdateUserType(UserTypeDTO obj, int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var result = _context.userTypeEntites.Where(a => a.UserTypeId == obj.UserTypeId && a.IsDeleted == false).FirstOrDefault();

                if (result == null)
                {
                    response.statuCode = 0;
                    response.message = "UserType not found";
                    return response;
                }

                int count = _context.userTypeEntites.Where(a => a.UserTypeName == obj.UserTypeName && a.UserTypeId != obj.UserTypeId && a.CreatedBy == id && a.IsDeleted == false).Count();

                if (count == 0)
                {
                    result.UserTypeName = obj.UserTypeName;
                    result.Description = obj.Description;
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
                    response.message = "User Type Already exists";
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Update UserType: " + ex.Message;
            }
            return response;
        }

        public GenericResponse DeleteUserType(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var result = _context.userTypeEntites.Where(a => a.UserTypeId == id && a.IsDeleted == false).FirstOrDefault();

                if (result == null)
                {
                    response.statuCode = 0;
                    response.message = "UserType not found";
                    return response;
                }

                result.IsDeleted = true;
                result.IsActive = false;
                result.UpdatedOn = DateTime.Now;

                _context.userTypeEntites.Update(result);
                _context.SaveChanges();

                response.statuCode = 1;
                response.message = "Success";
                response.currentId = result.UserTypeId;
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Delete UserType: " + ex.Message;
            }
            return response;
        }

        //UserMaster
        public List<UserDto> GetUser(int id)
        {
            try
            {
                var result = (from user in _context.userEntity
                              where user.IsDeleted == false && user.CreatedBy == id
                              select new UserDto
                              {
                                  UserId = user.UserId,
                                  FullName = user.FullName,
                                  Email = user.Email,
                                  ProfilePicture = user.ProfilePicture,
                                  UserType = _context.userTypeEntites.Where(a => a.UserTypeId == user.UserTypeId && a.IsDeleted == false).Select(a => a.UserTypeName).FirstOrDefault()
                              }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                // Log the exception and return empty list
                return new List<UserDto>();
            }
        }

        public GenericResponse AddUser(UserDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Check if user already exists
                    int count = _context.userEntity.Where(a => a.Email == obj.Email && a.IsDeleted == false).Count();

                    if (count > 0)
                    {
                        response.statuCode = 0;
                        response.message = "User with this email already exists";
                        return response;
                    }

                    UserEntity entity = new UserEntity();
                    entity.FullName = obj.FullName;
                    entity.Email = obj.Email;
                    entity.ProfilePicture = obj.ProfilePicture;
                    entity.UserTypeId = _context.userTypeEntites
                        .Where(a => a.UserTypeName == obj.UserType && a.CreatedBy == id && a.IsDeleted == false)
                        .Select(a => a.UserTypeId)
                        .FirstOrDefault();
                    entity.IsDeleted = false;
                    entity.IsActive = true;
                    entity.PasswordHash = EncryptTool.Encrypt(obj.PasswordHash);
                    entity.CreatedOn = DateTime.Now;
                    entity.CreatedBy = id;

                    _context.userEntity.Add(entity);
                    _context.SaveChanges();

                    // Add address if provided
                    if (!string.IsNullOrEmpty(obj.AddressLine))
                    {
                        AddressEntity address = new AddressEntity();
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
                    }

                    transaction.Commit();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = entity.UserId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.statuCode = 0;
                    response.message = "Failed to add User: " + ex.Message;
                }
            }
            return response;
        }

        public GenericResponse UpdateUser(UserDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var result = _context.userEntity.Where(a => a.UserId == obj.UserId && a.IsDeleted == false).FirstOrDefault();

                if (result == null)
                {
                    response.statuCode = 0;
                    response.message = "User not found";
                    return response;
                }

                // Check if another user with the same email exists
                int count = _context.userEntity.Where(a => a.Email == obj.Email && a.UserId != obj.UserId && a.IsDeleted == false).Count();

                if (count == 0)
                {
                    result.FullName = obj.FullName;
                    result.Email = obj.Email;
                    result.UserTypeId = _context.userTypeEntites
                        .Where(a => a.UserTypeName == obj.UserType && a.IsDeleted == false)
                        .Select(a => a.UserTypeId)
                        .FirstOrDefault();
                    result.PasswordHash = EncryptTool.Encrypt(obj.PasswordHash);
                    result.UpdatedOn = DateTime.Now;
                    result.UpdatedBy = id;

                    if (!string.IsNullOrEmpty(obj.ProfilePicture))
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
                    response.message = "User with this email already exists";
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Update User: " + ex.Message;
            }
            return response;
        }

        public GenericResponse DeleteUser(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                // First, let's check if the user exists without the IsDeleted filter
                var userExists = _context.userEntity.Any(a => a.UserId == id);

                if (!userExists)
                {
                    response.statuCode = 0;
                    response.message = "User with ID " + id + " does not exist in the system";
                    return response;
                }

                // Now check if user is already deleted
                var result = _context.userEntity.Where(a => a.UserId == id).FirstOrDefault();

                if (result == null)
                {
                    response.statuCode = 0;
                    response.message = "User not found";
                    return response;
                }

                if (result.IsDeleted == true)
                {
                    response.statuCode = 0;
                    response.message = "User is already deleted";
                    return response;
                }

                // Mark as deleted
                result.IsDeleted = true;
                result.IsActive = false;
                result.UpdatedOn = DateTime.Now;

                _context.userEntity.Update(result);
                _context.SaveChanges();

                response.statuCode = 1;
                response.message = "User deleted successfully";
                response.currentId = result.UserId;
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Delete User: " + ex.Message;
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
            try
            {
                DurationEntity entity = new DurationEntity();
                int count = _context.duration.Where(a => a.DurationType == obj.DurationType && a.CreatedBy == id && a.IsDeleted == false).Count();

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
                    response.message = "Duration Already exists";
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to add Duration: " + ex.Message;
            }
            return response;
        }

        public GenericResponse UpdateDuration(DurationDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var result = _context.duration.Where(a => a.Durationid == obj.Durationid && a.IsDeleted == false).FirstOrDefault();

                if (result == null)
                {
                    response.statuCode = 0;
                    response.message = "Duration not found";
                    return response;
                }

                int count = _context.duration.Where(a => a.DurationType == obj.DurationType && a.Durationid != obj.Durationid && a.IsDeleted == false).Count();

                if (count == 0)
                {
                    result.DurationType = obj.DurationType;
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
                    response.message = "Duration Already exists";
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Update Duration: " + ex.Message;
            }
            return response;
        }

        public GenericResponse DeleteDuration(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var result = _context.duration.Where(a => a.Durationid == id && a.IsDeleted == false).FirstOrDefault();

                if (result == null)
                {
                    response.statuCode = 0;
                    response.message = "Duration not found";
                    return response;
                }

                result.IsDeleted = true;
                result.IsActive = false;
                result.UpdatedOn = DateTime.Now;

                _context.duration.Update(result);
                _context.SaveChanges();

                response.statuCode = 1;
                response.message = "Success";
                response.currentId = result.Durationid;
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Delete Duration: " + ex.Message;
            }
            return response;
        }
    }
}