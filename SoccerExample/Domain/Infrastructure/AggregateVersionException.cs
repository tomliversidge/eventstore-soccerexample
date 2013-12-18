using System;

namespace Domain.Infrastructure
{
    public class AggregateVersionException : Exception
    {
        public int ExpectedVersion { get; set; }
        public int Version { get; set; }
        public Type Type { get; set; }
        public Guid Id { get; set; }

        public AggregateVersionException(Guid id, Type type, int version, int expectedVersion)
        {
            ExpectedVersion = expectedVersion;
            throw new NotImplementedException();
        }
    }
}