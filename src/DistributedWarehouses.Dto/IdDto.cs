using System;

namespace DistributedWarehouses.Dto
{
    public class IdDto
    {
        public IdDto(string id)
        {
            Id = Guid.Parse(id);
        }

        public IdDto(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}