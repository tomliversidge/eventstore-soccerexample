using System;

namespace Domain.Infrastructure
{
    public class AggregateNotFoundException : Exception
    {
        public Guid Id { get; set; }
        public Type Type { get; set; }

        public AggregateNotFoundException(Guid id, Type type)
        {
            Id = id;
            Type = type;
        }
    }
}