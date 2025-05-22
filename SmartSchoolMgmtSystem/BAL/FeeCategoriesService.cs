using SmartSchool.Utilities;
using SmartSchool.DAL;
using SmartSchool.Models.DTO;

namespace SmartSchool.BAL
{
    public class FeeCategoriesService :IFeeCategoriesService
    {
        private readonly IFeeCategoriesRepo _repository;
        public FeeCategoriesService(IFeeCategoriesRepo repository)
        {
            _repository = repository;
        }
        public List<FeeCatagoriesDto> GetFeeCategoryName(int id)
        {
            return _repository.GetFeeCategoryName(id);
        }
        public GenericResponse AddFeeCategory(FeeCatagoriesDto obj, int id)
        {
           GenericResponse response = new GenericResponse();
            response=_repository.AddFeeCategory(obj, id);
            return response;
        }
        public GenericResponse UpdateFeeCategory(FeeCatagoriesDto obj, int id)
        {
            GenericResponse response = new GenericResponse();
            response=_repository.UpdateFeeCategory(obj, id);
            return response;

        }
        public GenericResponse DeleteFeeCategory(int id)
        {
            GenericResponse response = new GenericResponse();
            response =_repository.DeleteFeeCategory(id);
            return response;
        }
    }
}
