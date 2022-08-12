using System;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace TestExample.Infrastructure.Web
{
    public class UriExpressionParser
    {
        public Expression<Func<TParameter, bool>> Parse<TParameter>(string expression, bool parseDateTimeAsUtc = false)
        {
            var config = new ParsingConfig
            {
                DateTimeIsParsedAsUTC = parseDateTimeAsUtc
            };
            return DynamicExpressionParser.ParseLambda<TParameter, bool>(
                config,
                false,
                expression);
        }
    }
}