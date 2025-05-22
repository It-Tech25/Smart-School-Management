
using SmartSchool.Utilities;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;

namespace SmartSchool.DAL
{
    public class FeeItemsRepo : IFeeItemsRepo
    {

        private readonly MyDbContext _context;
        private readonly IConfiguration _config;

        public FeeItemsRepo(MyDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
       
        
        public List<FeeItemsDto> GetFeeItems(int id)
        {
            var result = (from user in _context.feeItemEntity
                          where user.IsDeleted == false && user.CreatedBy == id
                          select new FeeItemsDto
                          {
                              ItemId = user.ItemId,
                              Name = user.Name,
                              CategoryId = user.CategoryId,
                              Amount=user.Amount
                          }).ToList();

            return result;
        }
        public GenericResponse AddFeeItems(FeeItemsDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            FeeItemsEntity entity = new FeeItemsEntity();
            int count = _context.feeItemEntity.Where(a => a.Name == obj.Name && a.IsDeleted == false).Count();
            int cid = _context.feeCategoriesEntity.Where(a => a.Name == obj.Name && a.IsDeleted == false).Select(a => a.CategoryId).FirstOrDefault();
            int itemid = _context.feeItemEntity.OrderByDescending(a => a.ItemId).Select(a => a.ItemId).FirstOrDefault();
            try
            {
                if (count < 1)
                {
                    entity.ItemId = itemid + 1;
                    entity.Name = obj.Name;
                    entity.CategoryId =obj.CategoryId ;
                    entity.Amount= obj.Amount;
                    entity.IsDeleted = false;
                    entity.CreatedOn = DateTime.Now;
                    entity.CreatedBy = id;
                    entity.UpdatedOn= DateTime.Now;
                    entity.UpdatedBy= id;

                    _context.feeItemEntity.Add(entity);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = entity.ItemId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = " Fee Items Alredy exist";
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to add Fee Items" + ex;
            }
            return response;
        }
        public GenericResponse UpdateFeeItems(FeeItemsDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.feeItemEntity.Where(a => a.ItemId == obj.ItemId && a.IsDeleted == false).FirstOrDefault();
            int count = _context.feeItemEntity.Where(a => a.Name == obj.Name).Count();
            try
            {
                if (count == 1)
                {
                    result.ItemId = obj.ItemId;
                    result.Name = obj.Name;
                    result.CategoryId = obj.CategoryId;
                    result.Amount = obj.Amount;
                    result.IsDeleted = false;
                    result.CreatedOn = result.CreatedOn;
                    result.CreatedBy = id;
                    result.UpdatedOn = DateTime.Now;
                    result.UpdatedBy = id;

                    _context.feeItemEntity.Update(result);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = result.ItemId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Fee Items Alredy exist";
                }

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to Update Fee Items" + ex;
            }
            return response;


        }
        public GenericResponse DeleteFeeItems(int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.feeItemEntity.Where(a => a.ItemId == id && a.IsDeleted == false).FirstOrDefault();
            try
            {
                result.IsDeleted = true;
                _context.feeItemEntity.Update(result);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Success";
                response.currentId = result.ItemId;

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Delete Fee Items" + ex;
            }
            return response;
        }

    }
}
