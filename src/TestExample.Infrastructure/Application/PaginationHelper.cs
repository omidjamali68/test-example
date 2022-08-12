using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TestExample.Infrastructure.Application
{
    public static class PaginationHelper
    {
        public static PageResult<T> PageResult<T>(
            this IQueryable<T> source, Pagination pagination)
        {
            return new PageResult<T>(source.Page(pagination).ToList(), source.Count());
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> source, Pagination pagination)
        {
            var query = source;

            if (pagination.PageNumber > 1) query = query.Skip((pagination.PageNumber - 1) * pagination.PageSize);

            query = query.Take(pagination.PageSize);

            return query;
        }

        public static async Task<PageResult<T>> Paginate<T>(
            this IQueryable<T> list, Pagination? pagination)
        {
            if (pagination != null)
            {
                var pageResult =
                    await list.Page(pagination).ToListAsync();
                return new PageResult<T>(
                    pageResult, pageResult.Count);
            }

            var resultList = await list.ToListAsync();
            return new PageResult<T>(
                resultList,
                resultList.Count);
        }
    }
}