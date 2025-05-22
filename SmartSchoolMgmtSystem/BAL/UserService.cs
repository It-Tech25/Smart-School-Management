using SmartSchool.DAL;
using SmartSchool.Models.Entity;
using SmartSchool.Models.DTO;
using SmartSchool.Utilities;
using Microsoft.EntityFrameworkCore;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Net.Mail;
using SmartSchool.BAL;
//using Microsoft.AspNetCore.Identity.Data;

namespace SmartSchool.BAL
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _UserRepo;
        private readonly MyDbContext _context;
        private readonly IConfiguration _config;
        private readonly ISubscriptionPaymentsService _Subscrioption;


        public UserService(IUserRepo UserRepo, MyDbContext contex, IConfiguration config,ISubscriptionPaymentsService Subscription)
        {
            _UserRepo = UserRepo;
            _context = contex;
            _config = config;
            _Subscrioption = Subscription;
        }

        //Login Check
        public LoginResponse LoginCheck(LoginRequest request)
        {
            return _UserRepo.LoginCheck(request);
        }
        //Count for SuperAdmin dashboard
        public int TotalSubscriptions()
        {
            return _UserRepo.TotalSubscriptions();
        }
        public List<SubscriptionPaymentsDto> GetSubscriptions()
        {
            var res = _Subscrioption.GetPaymentDetails();
            return res;
        }

        //User Type
        public List<UserTypeDTO> GetUserType(int id)
        {
            return _UserRepo.GetUserType(id);
        }
       public GenericResponse AddUserType(UserTypeDTO obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response=  _UserRepo.AddUserType(obj, id);
            return response;
        }
        public GenericResponse UpdateUserType(UserTypeDTO obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response = _UserRepo.UpdateUserType(obj, id);
            return response;
        }
        public GenericResponse DeleteUserType(int id)
        {
            GenericResponse response = new GenericResponse();
            response = _UserRepo.DeleteUserType(id);
            return response;
        }

        //User Master
        public List<UserDto> GetUser(int id)
        {
            return _UserRepo.GetUser(id);
        }
        public GenericResponse AddUser(UserDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response = _UserRepo.AddUser(obj, id);
            return response;
        }
        public GenericResponse UpdateUser(UserDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response = _UserRepo.UpdateUser(obj, id);
            return response;
        }
        public GenericResponse DeleteUser(int id)
        {
            GenericResponse response = new GenericResponse();
            response = _UserRepo.DeleteUser(id);
            return response;
        }

        public async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            // Query the database to find a user with the provided email and ensure that it's not deleted
            return await _context.userEntity
                                 .Where(u => u.Email == email && u.IsDeleted == false)
                                 .FirstOrDefaultAsync();
        }

        public bool PushEmail(string emailtext, string to, string subject, string cc = "", byte[] attachement = null)
        {
            bool res = false;
            try
            {

                // string emailimagesurl = _config.GetValue<string>("EmailConfig:EMAILIMAGEURL");
                string smtpserver = _config.GetValue<string>("EmailConfig:smtpServer");
                string smtpUsername = _config.GetValue<string>("EmailConfig:smtpEmail");
                string smtpPassword = _config.GetValue<string>("EmailConfig:smtppassword");
                int smtpPort = _config.GetValue<int>("EmailConfig:portNumber");

                // emailtext = emailtext.Replace("##EMAILIMAGES##", emailimagesurl);
                MailMessage msg = new MailMessage(smtpUsername, to, subject, emailtext);

                MailMessage mail = new MailMessage();
                mail.To.Add(to);
                if (!string.IsNullOrEmpty(cc))
                {
                    List<string> ccMails = cc.Split(",").ToList();
                    foreach (string e in ccMails)
                    {
                        mail.CC.Add(e);
                    }
                }

                mail.From = new MailAddress(smtpUsername);
                mail.Subject = subject;
                mail.Body = emailtext;
                mail.IsBodyHtml = true;
                if (attachement != null)
                {
                    using var attachment = new Attachment(new MemoryStream(attachement), "test.pdf");
                    mail.Attachments.Add(attachment);
                }
                SmtpClient smtp = new SmtpClient();
                smtp.Host = smtpserver;
                smtp.Port = smtpPort;

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
                smtp.EnableSsl = true;

                try
                {
                    smtp.Send(mail);

                    res = true;
                }
                catch (Exception ex)
                {
                    res = false;
                }



            }
            catch (Exception ex)
            {

                res = false;

            }
            return res;
        }
        public GenericResponse ResetPassword(UserEntity request)
        {
            GenericResponse response = new GenericResponse();
            try
            {

                UserEntity ue = new UserEntity();
                ue = _context.userEntity.Where(a => a.UserId == request.UserId).FirstOrDefault();

                ue.PasswordHash = request.PasswordHash;
                response = UpdateUsers(ue);
                if (response.statuCode == 1)
                {
                    // send email with otp to activate

                }
            }
            catch (Exception ex)
            {
                ex.Source = "Register User";

                response.statuCode = 0;
                response.message = "Failed to register";
            }
            return response;
        }

        public GenericResponse UpdateUsers(UserEntity request)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.userEntity.Where(a => a.Email == request.Email && a.IsDeleted == false).FirstOrDefault();
   
            try
            {
                    result.FullName = result.FullName;
                    result.Email = result.Email;
                result.UserTypeId = result.UserTypeId;
                    result.PasswordHash = EncryptTool.Encrypt(request.PasswordHash);
                    result.IsDeleted = false;
                    result.IsActive = true;
                    result.CreatedOn = result.CreatedOn;
                    result.CreatedBy = result.CreatedBy;
                    result.UpdatedOn = DateTime.Now;
                    result.UpdatedBy = result.UpdatedBy;
                    if (result.ProfilePicture == null)
                    {
                        result.ProfilePicture = result.ProfilePicture;
                    }
                  
                    _context.userEntity.Update(result);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = result.UserId;


            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Reset Password" + ex;
            }
            return response;
        
    }

        //Duration 

        public List<DurationDto> GetDuration()
        {
            return _UserRepo.GetDuration();
        }
        public GenericResponse AddDuration(DurationDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response = _UserRepo.AddDuration(obj, id);
            return response;
        }
        public GenericResponse UpdateDuration(DurationDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response = _UserRepo.UpdateDuration(obj, id);
            return response;
        }
        public GenericResponse DeleteDuration(int id)
        {
            GenericResponse response = new GenericResponse();
            response = _UserRepo.DeleteDuration(id);
            return response;
        }


    }
}
