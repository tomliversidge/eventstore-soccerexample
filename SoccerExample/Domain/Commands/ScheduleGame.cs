using System;
using Domain.Infrastructure;

namespace Domain.Commands
{
    public class ScheduleGame : Command
    {
        public Guid GameId { get; private set; }
        public Team HomeTeam { get; private set; }
        public Team AwayTeam { get; private set; }
        public DateTime KickOffTime { get; private set; }

        public ScheduleGame(Guid gameId, Team homeTeam, Team awayTeam, DateTime kickOffTime)
        {
            GameId = gameId;
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            KickOffTime = kickOffTime;
        }
    }
}