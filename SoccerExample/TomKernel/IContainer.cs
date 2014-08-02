using System.Collections.Generic;

namespace TomKernel
{
    public interface IContainer
    {
        IEnumerable<T> ResolveAll<T>();

        T Resolve<T>();
    }
}