using CarDealership.Core.Exceptions;
using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarDealership.DataAccess.Attributes
{
    public static class AttributesHelper<E> where E : BaseEntity
    {
        public static async Task EnsureAttributesUniqueness(DbSet<E> set, E entity, DbContext context)
        {
            var entityType = typeof(E);
            var parameter = Expression.Parameter(entityType, "x");

            foreach (var prop in entityType.GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(UniqueAttribute))))
            {
                var value = prop.GetValue(entity);
                if (value == null) continue;

                var property = Expression.Property(parameter, prop.Name);
                var constant = Expression.Constant(value);
                var equal = Expression.Equal(property, constant);
                var lambda = Expression.Lambda<Func<E, bool>>(equal, parameter);

                var existing = await set.FirstOrDefaultAsync(lambda);
                if (existing != null)
                {
                    if (existing.IsDeleted)
                    {
                        set.Remove(existing);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new ClientInformationException($"'{prop.Name}' должно быть уникальным. Значение '{value}' уже существует.");
                    }
                }
                    
            }

            var classLevelAttrs = Attribute.GetCustomAttributes(entityType, typeof(UniqueAttribute))
                .Cast<UniqueAttribute>();

            foreach (var attr in classLevelAttrs)
            {
                var comparisons = attr.PropertyNames.Select(name =>
                {
                    var property = entityType.GetProperty(name);
                    var value = property?.GetValue(entity);
                    if (property == null || value == null) return null;

                    return (Expression)Expression.Equal(
                        Expression.Property(parameter, name),
                        Expression.Constant(value));
                }).Where(c => c != null).ToList();

                if (!comparisons.Any()) continue;

                var combined = comparisons.Aggregate(Expression.AndAlso);
                var lambda = Expression.Lambda<Func<E, bool>>(combined, parameter);

                var existing = await set.FirstOrDefaultAsync(lambda);
                if (existing != null)
                {
                    if (existing.IsDeleted)
                    {
                        set.Remove(existing);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        var propsJoined = string.Join(", ", attr.PropertyNames);
                        throw new ClientInformationException($"Комбинация полей [{propsJoined}] должна быть уникальной.");
                    }
                }
            }
        }
    }
}
