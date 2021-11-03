using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DistributedWarehouses.Domain.Exceptions
{
    class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
