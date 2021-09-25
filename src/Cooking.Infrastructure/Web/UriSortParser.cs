using System.Linq;
using Cooking.Infrastructure.Application;

namespace Cooking.Infrastructure.Web
{
    public class UriSortParser
    {
        public Sort<T> Parse<T>(string expression)
        {
            var sortExprs = expression.Trim().Split(',');
            var sorts = sortExprs.Select(ExpressionToSort<T>);
            return sorts.Aggregate(
                (Sort<T>) null,
                (previous, current) => previous != null ? previous.And(current) : current);
        }

        private Sort<T> ExpressionToSort<T>(string expression)
        {
            var trimmedExpression = expression.Trim();
            var prefix = trimmedExpression[0];
            var propertyName = trimmedExpression.TrimStart('+', '-');

            if (prefix == '+')
                return Sort<T>.By(propertyName, SortDirection.Ascending);
            return Sort<T>.By(propertyName, SortDirection.Descending);
        }
    }
}