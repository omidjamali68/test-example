using Cooking.Entities.States;
using Cooking.Infrastructure.Application;
using Cooking.Services.StateServices.Cities.Contracts;

namespace Cooking.Services.StateServices.Cities
{
    public class CityAppService : ICityService
    {
        private readonly ICityRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CityAppService(ICityRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Register(RegisterCityDto dto)
        {
            var city = new City
            {
                Title = dto.Title,
                ProvinceId = dto.ProvinceId
            };

            _repository.Add(city);

            _unitOfWork.CompleteAsync();
        }

        public FindCityByIdDto FindById(int id)
        {
            return _repository.FindById(id);
        }
    }
}