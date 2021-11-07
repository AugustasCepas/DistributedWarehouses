namespace DistributedWarehouses.Domain.Services
{
    public interface IMappingService
    {
        TDestination Map<TDestination>(object source);
    }
}
