using SmartSchool.Utilities;
using SmartSchool.DAL;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
        public class SubscriptionsTypeServices : ISubscriptionsTypeServices
        {
            private readonly ISubscriptionsTypeRepo _SubscriptionsTypeRepo;

            public SubscriptionsTypeServices(ISubscriptionsTypeRepo SubscriptionsTypeRepo)
            {
            _SubscriptionsTypeRepo = SubscriptionsTypeRepo;
            }
            public List<string> GetStates()
            {
                return _SubscriptionsTypeRepo.GetStates();
            }
            public List<string> Getcity(string state)
            {
                return _SubscriptionsTypeRepo.Getcity(state);
            }
        // Get all SubscriptionsType
        public List<SubscriptionsTypeDto> GetAllSubscriptionsType()
            {
                return _SubscriptionsTypeRepo.GetSubscriptionsType();
            }

        // Add a new SubscriptionsType
        public GenericResponse AddSub(SubscriptionsTypeDto obj, int id)
            {
                GenericResponse response = new GenericResponse();
                response = _SubscriptionsTypeRepo.AddSubscriptionsType(obj, id);
                return response;
            }

        // Update SubscriptionsType
        public GenericResponse UpdateSubscriptionsType(SubscriptionsTypeDto obj, int id)
            {
                GenericResponse response = new GenericResponse();
                response = _SubscriptionsTypeRepo.UpdateSubscriptionsType(obj, id);
                return response;
            }

        // Delete SubscriptionsType
        public GenericResponse DeleteSubscriptionsType(int id)
            {
                GenericResponse response = new GenericResponse();
                response = _SubscriptionsTypeRepo.DeleteSubscriptionsType(id);
                return response;
            }
        }

    }
