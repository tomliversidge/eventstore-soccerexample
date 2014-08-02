using System;

namespace TomKernel
{
    public interface IRepository<T> where T : AggregateRoot
    {
        T GetById(Guid id);
        void Save(AggregateRoot aggregate);
    }
}