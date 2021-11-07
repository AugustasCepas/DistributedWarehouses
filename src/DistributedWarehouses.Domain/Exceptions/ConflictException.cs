using System.Net;

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
