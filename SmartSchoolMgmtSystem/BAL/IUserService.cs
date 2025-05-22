using SmartSchool.Models.Entity;
using SmartSchool.Utilities;
//using Microsoft.AspNetCore.Identity.Data;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public interface IUserService
    {
        //LogInCheck
        LoginResponse LoginCheck(LoginRequest request);

        //Count for SuperAdmin Dashboard
        int TotalSubscriptions();

        List<SubscriptionPaymentsDto> GetSubscriptions();

        //UserType
        List<UserTypeDTO> GetUserType(int id);
        GenericResponse AddUserType(UserTypeDTO obj, int id);
        GenericResponse UpdateUserType(UserTypeDTO obj, int id);
        GenericResponse DeleteUserType(int id);

        //User Master
        List<UserDto> GetUser(int id);
        GenericResponse AddUser(UserDto obj, int id);
        GenericResponse UpdateUser(UserDto obj, int id);
        GenericResponse DeleteUser(int id);

        Task<UserEntity> GetUserByEmailAsync(string email);
        GenericResponse UpdateUsers(UserEntity request);


        //Duration

        List<DurationDto> GetDuration();
        GenericResponse AddDuration(DurationDto obj, int id);
        GenericResponse UpdateDuration(DurationDto obj, int id);
        GenericResponse DeleteDuration(int id);
    }
}
