namespace DistributedWarehouses.Domain.Exceptions
{
    public class InsufficientStorageException : ConflictException
    {
        public InsufficientStorageException(int missingSpace) : base($"There is not enough space throughout all available warehouses. Reduce requested amount or add storage. Missing space: {missingSpace}")
        {
        }
    }
}
