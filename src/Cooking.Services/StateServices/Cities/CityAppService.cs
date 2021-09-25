using Cooking.Entities.States;
using Cooking.Infrastructure.Application;
using Cooking.Services.StateServices.Cities.Contracts;

namespace Cooking.Services.StateServices.Cities
{
    public class CityAppService : CityService
    {
        private readonly CityRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public CityAppService(CityRepository repository, UnitOfWork unitOfWork)
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

            _unitOfWork.Complete();
        }

        public FindCityByIdDto FindById(int id)
        {
            return _repository.FindById(id);
        }
    }
}