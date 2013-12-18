using System;

namespace Domain.Infrastructure
{
    public interface IHandle<T> where T : IMessage
    {
        void Handle(T args);
    }
}