
using SmartSchool.Utilities;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;

namespace SmartSchool.DAL
{
    public class ModulesRepo : IModulesRepo
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _config;

        public ModulesRepo(MyDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public List<ModulesDto> GetModule(int id)
        {
            var result = (from user in _context.module
                          where user.IsDeleted == false && user.CreatedBy == id
                          select new ModulesDto
                          {
                             ModuleId = user.ModuleId,
                              Name = user.Name,
                          }).ToList();

            return result;
        }
        public GenericResponse AddModule(ModulesDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            ModulesEntity entity = new ModulesEntity();
            int count = _context.module.Where(a => a.Name == obj.Name && a.CreatedBy == id && a.IsDeleted == false).Count();
            int Mid = _context.module.OrderByDescending(a => a.ModuleId).Select(a => a.ModuleId).FirstOrDefault();
            try
            {
                if (count == 0)
                {
                    entity.ModuleId = Mid + 1;
                    entity.Name = obj.Name;
                    entity.IsDeleted = false;
                    entity.IsActive = true;
                    entity.CreatedOn = DateTime.Now;
                    entity.CreatedBy = id;

                    _context.module.Add(entity);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = entity.ModuleId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Module Alredy exist";
                }
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to add Module" + ex;
            }
            return response;
        }
        public GenericResponse UpdateModule(ModulesDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.module.Where(a => a.ModuleId == obj.ModuleId && a.IsDeleted == false).FirstOrDefault();
            int count = _context.module.Where(a => a.Name == obj.Name).Count();
            try
            {
                if (count < 1)
                {
                    result.ModuleId = obj.ModuleId;
                    result.Name = obj.Name;
                    result.IsDeleted = false;
                    result.IsActive = true;
                    result.CreatedOn = result.CreatedOn;
                    result.UpdatedOn = DateTime.Now;
                    result.UpdatedBy = id;

                    _context.module.Update(result);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "Success";
                    response.currentId = result.ModuleId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Module Alredy exist";
                }

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "failed to Update Module" + ex;
            }
            return response;

        }
        public GenericResponse DeleteModule(int id)
        {
            GenericResponse response = new GenericResponse();
            var result = _context.module.Where(a => a.ModuleId == id && a.IsDeleted == false).FirstOrDefault();
            try
            {
                result.IsDeleted = true;
                result.IsActive = false;
                _context.module.Update(result);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "Success";
                response.currentId = result.ModuleId;

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to Delete Module" + ex;
            }
            return response;
        }
    }
}
