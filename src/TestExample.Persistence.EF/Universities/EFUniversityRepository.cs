using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestExample.Entities.Universities;
using TestExample.Infrastructure;
using TestExample.Infrastructure.Application;
using TestExample.Services.Universities.Contracts;

namespace TestExample.Persistence.EF.Universities
{
    public class EFUniversityRepository : IUniversityRepository
    {
        private readonly DbSet<University> _universities;

        public EFUniversityRepository(EFDataContext context)
        {
            _universities = context.Set<University>();
        }

        public void Add(University university)
        {
            _universities.Add(university);
        }

        public async Task<University> FindById(int id)
        {
            return await _universities.FindAsync(id);
        }

        public async Task<PageResult<GetAllUniversitiesDto>> GetAll(
            Pagination? pagination = null,
            Sort<GetAllUniversitiesDto>? sort = null,
            string? search = null)
        {
            var result = _universities.Select(_ => new GetAllUniversitiesDto
            {
                Id = _.Id,
                Address = _.Address,
                Email = _.Email,
                Name = _.Name
            });

            result = FilterSearchText(result, search);

            if (sort != null)
                result = result.Sort(sort);

            return await result.Paginate(pagination);
        }

        public async Task<GetUniversityDto> GetById(int id)
        {
            return await _universities.Where(_ => _.Id == id)
                .Select(_ => new GetUniversityDto
                {
                    Address = _.Address,
                    Email = _.Email,
                    Name = _.Name
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> IsNameExist(string name, int? id = null)
        {
            var normalizeName = name.NormalizeText();
            return await _universities.AnyAsync(_ =>
                _.Name.Replace(" ", "").ToLower().Equals(normalizeName) &&
                _.Id != id);
        }

        public void Delete(University university)
        {
            _universities.Remove(university);
        }

        private static IQueryable<GetAllUniversitiesDto> FilterSearchText(
            IQueryable<GetAllUniversitiesDto> result, string search)
        {
            var normalizeSearch = search.NormalizeText();
            return result.Where(_ => _.Name.Replace(" ", "").ToLower()
                .Contains(normalizeSearch));
        }
    }
}