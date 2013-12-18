using System;
using Domain.Infrastructure;

namespace Domain.Commands
{
    public class BlowFullTimeWhistle : Command
    {
        public Guid GameId { get; private set; }
        public DateTime Time { get; private set; }

        public BlowFullTimeWhistle(Guid gameId, DateTime time)
        {
            GameId = gameId;
            Time = time;
        }
    }
}