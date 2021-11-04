using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Resources;

namespace DistributedWarehouses.Domain.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string param) : base(string.Format(ErrorMessageResource.NotFound, param))
        {
            StatusCode = HttpStatusCode.NotFound;
        }
    }
}
