using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Models.DTO
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }
        public string? UserType { get; set; }

        public string? PasswordHash { get; set; }
        public string? ProfilePicture { get; set; }

        public int UserTypeId { get; set; }
        public int AddressId { get; set; }


        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PinCode { get; set; }


    }

    public class RegisterStepFinal
    {
        public int UserId { get; set; }
        public string mobileOtp { get; set; }
        public string emailOtp { get; set; }
        public string UserName { get; set; }
        public string emailId { get; set; }
        public string mobileNumber { get; set; }

        public int OTPTrid { get; set; }

    }

    public class ResetPasswordFinal
    {
        public int UserId { get; set; }
        public string mobileOtp { get; set; }
        [Required(ErrorMessage = "OTP required")]
        public string emailOtp { get; set; }
        public int OTPTrid { get; set; }

        [StringLength(100, ErrorMessage = "Please enter at least 6 characters.", MinimumLength = 6)]

        [Required(ErrorMessage = "Password required")]
        public string pword { get; set; }
        [Required(ErrorMessage = "Confirm Password required")]
        [Compare("pword", ErrorMessage = "Passwords mismatch")]
        public string cnfpword { get; set; }
        public string UserName { get; set; }
    }


}
