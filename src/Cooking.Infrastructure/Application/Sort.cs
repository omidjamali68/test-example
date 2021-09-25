using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Cooking.Infrastructure.Application
{
    public class Sort<T>
    {
        private readonly Dictionary<MemberInfo, SortDirection> _orders;

        private Sort(IEnumerable<KeyValuePair<MemberInfo, SortDirection>> orders)
        {
            _orders = orders.GroupBy(_ => _.Key).ToDictionary(_ => _.Key, _ => _.Last().Value);
        }

        public (MemberInfo Property, SortDirection Direction)[] Orders
        {
            get { return _orders.Select(_ => (_.Key, _.Value)).ToArray(); }
        }

        public Sort<T> And(Sort<T> sort)
        {
            return new Sort<T>(_orders.Concat(sort._orders));
        }

        public Sort<T> Ascending()
        {
            return new Sort<T>(_orders.Select(_ =>
                new KeyValuePair<MemberInfo, SortDirection>(_.Key, SortDirection.Ascending)));
        }

        public Sort<T> Descending()
        {
            return new Sort<T>(_orders.Select(_ =>
                new KeyValuePair<MemberInfo, SortDirection>(_.Key, SortDirection.Descending)));
        }

        public static Sort<T> By<TProperty>(Expression<Func<T, TProperty>> property)
        {
            return By(property, SortDirection.Ascending);
        }

        public static Sort<T> By<TProperty>(Expression<Func<T, TProperty>> property, SortDirection direction)
        {
            var memberExpression = property.Body.UnwrapQuote() as MemberExpression;
            return new Sort<T>(new[]
            {
                new KeyValuePair<MemberInfo, SortDirection>(memberExpression.Member, direction)
            });
        }

        public static Sort<T> By(string propertyName)
        {
            return By(propertyName, SortDirection.Ascending);
        }

        public static Sort<T> By(string propertyName, SortDirection direction)
        {
            var property = ResolveProperty(propertyName);
            return new Sort<T>(new[]
            {
                new KeyValuePair<MemberInfo, SortDirection>(property, direction)
            });
        }

        private static MemberInfo ResolveProperty(string propertyName)
        {
            var type = typeof(T);
            var searchFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;
            return (MemberInfo) type.GetProperty(propertyName, searchFlags) ?? type.GetField(propertyName, searchFlags);
        }
    }
}