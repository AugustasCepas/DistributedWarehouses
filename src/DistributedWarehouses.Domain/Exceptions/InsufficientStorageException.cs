using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Resources;

namespace DistributedWarehouses.Domain.Exceptions
{
    public class InsufficientStorageException : Exception
    {
        public InsufficientStorageException(string message) : base(message)
        { 
        }
    }
}
