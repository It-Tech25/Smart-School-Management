using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.DAL
{
    public interface ISubscriptionsTypeRepo
    {
        List<string> GetStates();
        List<string> Getcity(string state);
        List<SubscriptionsTypeDto> GetSubscriptionsType();
        GenericResponse AddSubscriptionsType(SubscriptionsTypeDto obj, int id);
        GenericResponse UpdateSubscriptionsType(SubscriptionsTypeDto obj, int id);
        GenericResponse DeleteSubscriptionsType(int id);
       
    }
}
