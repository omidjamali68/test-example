using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Cooking.Entities.States;
using Cooking.Infrastructure.Application;
using Cooking.Services.StateServices.Nationalities.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Cooking.Persistence.EF.StatePersistence.Nationalities
{
    public class EFNationalityRepository : INationalityRepository
    {
        private readonly DbSet<Nationality> _nationalities;

        public EFNationalityRepository(EFDataContext context)
        {
            _nationalities = context.Set<Nationality>();
        }

        public async Task<PageResult<GetAllNationalityDto>> GetAll(
            string searchText, 
            Pagination pagination, 
            Sort<GetAllNationalityDto> sortExpression)
        {
            var result = _nationalities.Select(_ => new GetAllNationalityDto{
                Id = _.Id,
                Name = _.Name
            });

            if (!string.IsNullOrWhiteSpace(searchText))
                result = SearchResultForText(searchText, result);
            
            if (sortExpression != null)
                result = result.Sort(sortExpression);


            var resultList = await result.ToListAsync();
            return new PageResult<GetAllNationalityDto>(resultList, resultList.Count);
        }

        private IQueryable<GetAllNationalityDto> SearchResultForText(string searchText,
          IQueryable<GetAllNationalityDto> result)
        {
            return result.Where(_ => _.Name.Replace(" ","").Contains(searchText.Replace(" ", "")));
        }
    }
}