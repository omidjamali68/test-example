using System;
using System.Threading.Tasks;
using TestExample.Entities.Universities;
using TestExample.Infrastructure.Application;
using TestExample.Services.Universities.Contracts;
using TestExample.Services.Universities.Exceptions;

namespace TestExample.Services.Universities
{
    public class UniversityAppService : IUniversityService
    {
        private readonly IUniversityRepository _universityRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UniversityAppService(
            IUniversityRepository universityRepository,
             IUnitOfWork unitOfWork)
        {
            _universityRepository = universityRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(AddUniversityDto dto)
        {
            await StopIfNameExist(dto.Name);
            var university = new University(
                dto.Name, dto.Address, dto.Email);
            _universityRepository.Add(university);

            await _unitOfWork.CompleteAsync();

            return university.Id;
        }

        public async Task Update(int id, UpdateUniversityDto dto)
        {
            await StopIfNameExist(dto.Name, id);
            var university = await _universityRepository.FindById(id);
            StopIfUniversityNotFound(university);

            university.Address = dto.Address;
            university.Email = dto.Email;
            university.Name = dto.Name;

            await _unitOfWork.CompleteAsync();
        }

        public async Task<GetUniversityDto> GetById(int id)
        {
            return await _universityRepository.GetById(id);
        }        

        public async Task<PageResult<GetAllUniversitiesDto>> GetAll(
            Pagination? pagination = null,
            Sort<GetAllUniversitiesDto>? sort = null,
            string? search = null)
        {
            return await _universityRepository.GetAll(
                pagination, sort, search);
        }

        public async Task Delete(int id)
        {
            var university = await _universityRepository.FindById(id);
            StopIfUniversityNotFound(university);

            _universityRepository.Delete(university);
            await _unitOfWork.CompleteAsync();
        }

        private static void StopIfUniversityNotFound(University university)
        {
            if (university is null)
                throw new UniversityNotFoundException();
        }

        private async Task StopIfNameExist(string name, int? id = null)
        {
            if (await _universityRepository.IsNameExist(name, id))
                throw new UniversityNameExistException();
        }
    }
}