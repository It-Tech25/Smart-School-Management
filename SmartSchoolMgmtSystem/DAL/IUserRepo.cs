using SmartSchool.Utilities;
//using Microsoft.AspNetCore.Identity.Data;
using SmartSchool.Models.DTO;

namespace SmartSchool.DAL
{
    public interface IUserRepo
    {
        LoginResponse LoginCheck(LoginRequest request);

        //Count for Super Admin Dashboard
        int TotalSubscriptions();

        //User Type
        List<UserTypeDTO> GetUserType(int id);
        GenericResponse AddUserType(UserTypeDTO obj, int id);
        GenericResponse UpdateUserType(UserTypeDTO obj, int id);
        GenericResponse DeleteUserType(int id);

        //User Master
        List<UserDto> GetUser(int id);
        GenericResponse AddUser(UserDto obj, int id);
        GenericResponse UpdateUser(UserDto obj, int id);
        GenericResponse DeleteUser(int id);

        //Duration

        List<DurationDto> GetDuration();
        GenericResponse AddDuration(DurationDto obj, int id);
        GenericResponse UpdateDuration(DurationDto obj, int id);
        GenericResponse DeleteDuration(int id);
    }
}
