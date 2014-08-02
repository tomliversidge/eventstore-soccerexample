using System;
using TomKernel;

namespace Domain.Events
{
    public class GameScheduled : Event
    {
        public Guid Id { get; private set; }
        public Team HomeTeam { get; private set; }
        public Team AwayTeam { get; private set; }
        public DateTime KickOffTime { get; private set; }

        public GameScheduled(Guid id, Team homeTeam, Team awayTeam, DateTime kickOffTime)
        {
            Id = id;
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            KickOffTime = kickOffTime;
        }
    }
}
