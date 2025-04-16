using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using System.Linq.Expressions;
using System.Reflection;

namespace CarDealership.DataAccess.Factories
{
     public static class SqlProjectionFactory
    {
        public static Expression<Func<E, M>> CreateProjectionExpression<E, M>()
            where E : BaseEntity
            where M : BaseModel
        {
            var entityProperties = typeof(E).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var modelProperties = typeof(M).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var entityParam = Expression.Parameter(typeof(E), "e");

            var bindings = modelProperties
                .Where(modelProp => entityProperties
                    .Any(entityProp => entityProp.Name == modelProp.Name && entityProp.PropertyType == modelProp.PropertyType))
                .Select(modelProp =>
                {
                    var entityProp = entityProperties.Single(p => p.Name == modelProp.Name);

                    var entityPropAccess = Expression.Property(entityParam, entityProp);
                    var modelPropAccess = Expression.PropertyOrField(entityParam, modelProp.Name);

                    return Expression.Bind(modelProp, entityPropAccess);
                })
                .ToList();

            var newExpression = Expression.New(typeof(M));
            var memberInit = Expression.MemberInit(newExpression, bindings);

            return Expression.Lambda<Func<E, M>>(memberInit, entityParam);
        }
    }
}
