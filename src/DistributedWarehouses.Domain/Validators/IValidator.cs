using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedWarehouses.Domain.Validators
{
    public interface IValidator<T, TRepository>
    {
        public  Task ValidateAsync(T param);
    }
}
