using System;

namespace TomKernel.Exceptions
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