using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Cooking.Infrastructure
{
    public static class EFDataContextHelper
    {
        public static IQueryable Set(this DbContext context, Type entityType)
        {
            MethodInfo method =
                    typeof(DbContext).GetMethods()
                    .FirstOrDefault(_ => _.Name == nameof(DbContext.Set));

            method = method.MakeGenericMethod(entityType);

            return method.Invoke(context, null) as IQueryable;
        }
    }
}
