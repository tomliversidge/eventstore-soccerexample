using System.Collections.Generic;

namespace Domain.Infrastructure
{
    public interface IContainer
    {
        IEnumerable<T> ResolveAll<T>();

        T Resolve<T>();
    }
}