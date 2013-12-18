using System;

namespace Domain.Infrastructure
{
    public class Event : IEvent
    {
        public Guid Id;

        public Event()
        {
            Id = Guid.NewGuid();
        }
    }
}