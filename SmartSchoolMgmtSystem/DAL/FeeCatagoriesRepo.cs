 
using SmartSchool.Utilities;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;

namespace SmartSchool.DAL
{
    public class FeeCatagoriesRepo : IFeeCategoriesRepo
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _config;

        public FeeCatagoriesRepo(MyDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public List<FeeCatagoriesDto> GetFeeCategoryName(int id)
        {
            var result = (from user in _context.feeCategoriesEntity
                          where user.IsDeleted == false && user.CreatedBy == id
                          select new FeeCatagoriesDto
                          {
                              CategoryId = user.CategoryId,
                              Name = user.Name,
                             
                          }).ToList();

            return result;
        }
        public GenericResponse AddFeeCategory(FeeCatagoriesDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            FeeCategoriesEntity entity = new FeeCategoriesEntity();
            int count = _context.feeCategoriesEntity.Where(a => a.Name == obj.Name && a.CreatedBy == id && a.IsDeleted == false).Count();
            int feeid = _context.feeCategoriesEntity.OrderByDescending(a => a.CategoryId).Select(a => a.CategoryId).FirstOrDefault();
            try
            {
                if (count == 0)
                {
                    entity.CategoryId = feeid + 1;
                    entity.Name = obj.Name;
                    entity.IsDeleted = false;
                    entity.CreatedOn = DateTime.Now;
                    entity.CreatedBy = id;
                   

                    _context.feeCategoriesEntity.Add(entity);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = entity.CategoryId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Fee Category Type Alredy exist";
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to add Fee Category" + ex;
            }
            return response;
        }
        public GenericResponse UpdateFeeCategory(FeeCatagoriesDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.feeCategoriesEntity.Where(a => a.CategoryId == obj.CategoryId && a.IsDeleted == false).FirstOrDefault();
            int count = _context.feeCategoriesEntity.Where(a => a.Name == obj.Name).Count();
            try
            {
                if (count <=1)
                {
                    result.CategoryId = obj.CategoryId;
                    result.Name = obj.Name;
                    result.IsDeleted = false;
                    result.CreatedOn = result.CreatedOn;
                    result.UpdatedOn = DateTime.Now;
                    result.UpdatedBy = id;

                    _context.feeCategoriesEntity.Update(result);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = result.CategoryId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Fee Category Name Alredy exist";
                }

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to Update Fee Category Name" + ex;
            }
            return response;

        }
        public GenericResponse DeleteFeeCategory(int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.feeCategoriesEntity.Where(a => a.CategoryId == id && a.IsDeleted == false).FirstOrDefault();
            try
            {
                result.IsDeleted = true;
                _context.feeCategoriesEntity.Update(result);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Success";
                response.currentId = result.CategoryId;

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Delete Fee Category Name" + ex;
            }
            return response;
        }
    }
}
