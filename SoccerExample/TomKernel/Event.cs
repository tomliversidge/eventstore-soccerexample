using System;

namespace TomKernel
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