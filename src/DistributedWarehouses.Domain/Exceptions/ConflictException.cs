using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DistributedWarehouses.Domain.Exceptions
{
    public class ConflictException : BaseException
    {
        public ConflictException(string message) : base(message)
        {
            StatusCode = HttpStatusCode.Conflict;
        }
    }
}
