using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Resources;

namespace DistributedWarehouses.Domain.Exceptions
{
    public class InsufficientStorageException : ConflictException
    {
        public InsufficientStorageException(int missingSpace) : base($"There is not enough space throughout all available warehouses. Reduce requested amount or add storage. Missing space: {missingSpace}")
        {
        }
    }
}
