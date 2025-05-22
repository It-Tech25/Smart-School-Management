
using SmartSchool.Utilities;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;

namespace SmartSchool.DAL
{
    public class SubscriptionsTypeRepo : ISubscriptionsTypeRepo
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _config;

        public SubscriptionsTypeRepo(MyDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        public List<string> GetStates()
        {
            var result = _context.stateEntity.Select(a => a.StateName).ToList();
            return result;
        }

        public List<string> Getcity(string state)
        {
            int id = _context.stateEntity.Where(a => a.StateName == state).Select(a => a.StateId).FirstOrDefault();
            var result = _context.cityEntities.Where(a => a.StateId == id).Select(a => a.CityName).ToList();
            return result;
        }


        public List<SubscriptionsTypeDto> GetSubscriptionsType()
        {
            var result = (from SubscriptionsType in _context.subscriptionsTypeEntity
                          where SubscriptionsType.IsDeleted == false
                          select new SubscriptionsTypeDto
                          {
                             SubscriptionTypeId = SubscriptionsType.SubscriptionTypeId,
                              ModulesEnabled = SubscriptionsType.ModulesEnabled
                          }).ToList();
            return result;
        }


        public GenericResponse AddSubscriptionsType(SubscriptionsTypeDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            SubscriptionsTypeEntity entity = new SubscriptionsTypeEntity();
            int SubscriptionTypeId= _context.subscriptionsTypeEntity.OrderByDescending(a => a.SubscriptionTypeId).Select(a => a.SubscriptionTypeId).FirstOrDefault();
            int count = _context.subscriptionsTypeEntity.Where(a => a.SubscriptionTypeId == obj.SubscriptionTypeId && a.CreatedBy == id && a.IsDeleted == false).Count();
        try
        {
            if (count < 1)
            {
                entity.SubscriptionTypeId = SubscriptionTypeId+1;
                entity.ModulesEnabled = obj.ModulesEnabled;
                entity.IsActive = true;
                entity.IsDeleted = false;
                entity.CreatedOn = DateTime.Now;
                entity.CreatedBy = id;
                entity.UpdatedOn = DateTime.Now;
                entity.UpdatedBy = id;

                _context.subscriptionsTypeEntity.Add(entity);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Success";
                response.currentId = entity.SubscriptionTypeId;
            }
            else
            {
                response.statuCode = 0;
                response.message = "SubscriptionsType Alredy exist";
            }
        }
        catch (Exception ex)
        {
            response.statuCode = 0;
            response.message = "failed to add SubscriptionsType" + ex;
        }
            return response;
        }

        public GenericResponse UpdateSubscriptionsType(SubscriptionsTypeDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.subscriptionsTypeEntity.Where(a => a.SubscriptionTypeId == obj.SubscriptionTypeId && a.IsDeleted == false).FirstOrDefault();
            int count = _context.subscriptionsTypeEntity.Where(a => a.SubscriptionTypeId == obj.SubscriptionTypeId).Count();
            try
            {
                if (count == 1)
                {
                    result.SubscriptionTypeId = obj.SubscriptionTypeId;
                    result.ModulesEnabled = obj.ModulesEnabled;
                    result.IsActive = true;
                    result.IsDeleted = false;
                    result.CreatedOn = DateTime.Now;
                    result.CreatedBy = id;
                    result.UpdatedOn = DateTime.Now;
                    result.UpdatedBy = id;
                    _context.subscriptionsTypeEntity.Update(result);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = result.SubscriptionTypeId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "SubscriptionsType Alredy exist";
                }

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to Update SubscriptionsType" + ex;
            }
            return response;
        }

        public GenericResponse DeleteSubscriptionsType(int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.subscriptionsTypeEntity.Where(a => a.SubscriptionTypeId == id && a.IsDeleted == false).FirstOrDefault();
            try
            {
                result.IsDeleted = true;
                _context.subscriptionsTypeEntity.Update(result);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Success";
                response.currentId = result.SubscriptionTypeId;

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Delete SubscriptionsType" + ex;
            }
            return response;
        }

    }

}