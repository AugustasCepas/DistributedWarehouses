namespace DistributedWarehouses.Domain.Exceptions
{
    public class InsufficientAmountException : ConflictException
    {
        public InsufficientAmountException(string message) : base(message)
        {
        }
    }
}
