using SmartSchool.Utilities;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public interface ISubscriptionsTypeServices
    {
        List<string> GetStates();
        List<string> Getcity(string state);
        // Get all SubscriptionsType
        List<SubscriptionsTypeDto> GetAllSubscriptionsType();
        // Add a new SubscriptionsType
        GenericResponse AddSub(SubscriptionsTypeDto obj, int id);
        // Update SubscriptionsType
        GenericResponse UpdateSubscriptionsType(SubscriptionsTypeDto obj, int id);
        // Delete SubscriptionsType
        GenericResponse DeleteSubscriptionsType(int id);
    }
}
