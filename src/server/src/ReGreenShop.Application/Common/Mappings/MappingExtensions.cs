using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;

namespace ReGreenShop.Application.Common.Mappings;
public static class MappingExtensions
{
    public static IQueryable<TDestination> To<TDestination>(
           this IQueryable source,
           params Expression<Func<TDestination, object>>[] membersToExpand)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return source.ProjectTo(AutoMapperConfig.MapperInstance.ConfigurationProvider, null, membersToExpand);
    }

    public static IQueryable<TDestination> To<TDestination>(
        this IQueryable source,
        object parameters)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return source.ProjectTo<TDestination>(AutoMapperConfig.MapperInstance.ConfigurationProvider, parameters);
    }
}
