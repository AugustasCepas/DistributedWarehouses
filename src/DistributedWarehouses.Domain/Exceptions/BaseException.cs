using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DistributedWarehouses.Domain.Exceptions
{
    public class BaseException :Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public BaseException(string message) : base(message)
        {
        }
        public BaseException(string message, int code) : base(message)
        {
            StatusCode = (HttpStatusCode)code;
        }
    }
}
