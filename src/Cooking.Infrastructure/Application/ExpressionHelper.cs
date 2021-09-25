using System;
using System.Linq;
using System.Linq.Expressions;

namespace Cooking.Infrastructure.Application
{
    public static class ExpressionHelper
    {
        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> source,
            Expression<Func<T, bool>> expression)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var parameter = source.Parameters.First();
            var leftExp = source.Body.UnwrapQuote();
            var rightExp = ExpressionReplacer.Replace(
                expression.Body.UnwrapQuote(),
                expression.Parameters.First(),
                parameter);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(leftExp, rightExp), parameter);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> source,
            Expression<Func<T, bool>> expression)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var parameter = source.Parameters.First();
            var leftExp = source.Body.UnwrapQuote();
            var rightExp = ExpressionReplacer.Replace(
                expression.Body.UnwrapQuote(),
                expression.Parameters.First(),
                parameter);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(leftExp, rightExp), parameter);
        }

        public static Expression<Func<T, bool>> OrNot<T>(this Expression<Func<T, bool>> source,
            Expression<Func<T, bool>> expression)
        {
            return source.Or(expression.Not());
        }

        public static Expression<Func<T, bool>> AndNot<T>(this Expression<Func<T, bool>> source,
            Expression<Func<T, bool>> expression)
        {
            return source.And(expression.Not());
        }

        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return Expression.Lambda<Func<T, bool>>(
                Expression.Not(expression.Body.UnwrapQuote()),
                expression.Parameters.First());
        }

        internal static Expression UnwrapQuote(this Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (expression.NodeType == ExpressionType.Quote)
                return (expression as UnaryExpression).Operand.UnwrapQuote();

            return expression;
        }
    }
}