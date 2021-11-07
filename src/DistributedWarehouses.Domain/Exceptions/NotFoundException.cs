using System.Net;
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
