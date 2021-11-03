using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedWarehouses.Domain.Services
{
    public interface IMappingService
    {
        TDestination Map<TDestination>(object source);
    }
}
