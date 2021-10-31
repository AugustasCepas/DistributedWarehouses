using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedWarehouses.Domain.Exceptions
{
    public class InsufficientAmountException : Exception
    {
        public InsufficientAmountException(string message) : base(message)
        {
        }
    }
}
