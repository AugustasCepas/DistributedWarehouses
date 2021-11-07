using AutoMapper;
using AutoMapper.Data;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Infrastructure.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedWarehouses.Api.Injections
{
    public static class MappingInjection
    {
        public static IServiceCollection AddMapping(this IServiceCollection services)
        {
            return services
                .AddAutoMapper(ApplyProfiles)
                .AddSingleton<IMappingService, MappingService>();
        }

        public static void ApplyProfiles(IMapperConfigurationExpression config)
        {
            config.AddDataReaderMapping();
            config.AddProfile<MappingProfile>();
        }
    }
}
