using System.Collections.Generic;
using Cooking.Entities.States;
using Cooking.Infrastructure.Application;
using Cooking.Services.StateServices.Provinces.Contracts;
using Cooking.Services.StateServices.Provinces.Exceptions;

namespace Cooking.Services.StateServices.Provinces
{
    public class ProvinceAppService : ProvinceService
    {
        private readonly ProvinceRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public ProvinceAppService(ProvinceRepository repository, UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Register(RegisterProvinceDto dto)
        {
            if (_repository.ExistsByName(dto.Title)) throw new DuplicateProvinceNameException {Name = dto.Title};

            var province = new Province
            {
                Title = dto.Title
            };

            _repository.Add(province);

            _unitOfWork.Complete();
        }

        public IList<GetAllProvincesDto> GetAll()
        {
            return _repository.GetAllProvinces();
        }

        public IList<GetProvinceCitiesDto> GetCities(int id)
        {
            return _repository.GetCities(id);
        }
    }
}