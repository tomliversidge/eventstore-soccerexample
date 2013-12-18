using System;
using Domain.Infrastructure;

namespace Domain.Events
{
    public class KickedOff : Event
    {
        public Guid GameId { get; set; }
        public DateTime StartedAt { get; set; }

        public KickedOff(Guid gameId, DateTime startedAt)
        {
            GameId = gameId;
            StartedAt = startedAt;
        }
    }
}