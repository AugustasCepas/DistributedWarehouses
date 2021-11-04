using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedWarehouses.ApplicationServices.Validators;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedWarehouses.ApplicationServices
{
    public class RepositoryProvider : IRepositoryProvider
    {
        private readonly IServiceProvider _provider;
        public RepositoryProvider(IServiceProvider provider)
        {
            _provider = provider;
        }
        public T GetRepository<T>() where T : IRepository
        {
            return _provider.GetRequiredService<T>();
        }
    }
}
