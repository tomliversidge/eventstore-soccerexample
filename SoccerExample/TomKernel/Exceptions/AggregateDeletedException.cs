using System;

namespace TomKernel.Exceptions
{
    public class AggregateDeletedException : Exception
    {
        public Guid Id { get; set; }
        public Type Type { get; set; }

        public AggregateDeletedException(Guid id, Type type)
        {
            Id = id;
            Type = type;
        }
    }
}