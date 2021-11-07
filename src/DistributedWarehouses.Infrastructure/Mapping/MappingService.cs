using AutoMapper;
using DistributedWarehouses.Domain.Services;

namespace DistributedWarehouses.Infrastructure.Mapping
{
    public class MappingService : IMappingService
    {
        private readonly IMapper _mapper;
        public MappingService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }
    }
}
