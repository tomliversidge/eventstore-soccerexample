using System;
using Domain.Infrastructure;

namespace Domain.Commands
{
    public class KickOff : Command
    {
        public Guid GameId { get; private set; }
        public DateTime Time { get; private set; }

        public KickOff(Guid gameId, DateTime time)
        {
            GameId = gameId;
            Time = time;
        }
    }
}