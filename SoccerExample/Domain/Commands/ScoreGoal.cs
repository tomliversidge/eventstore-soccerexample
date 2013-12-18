using System;
using Domain.Infrastructure;

namespace Domain.Commands
{
    public class ScoreGoal : Command
    {
        public Guid GameId { get; private set; }
        public Team Team { get; private set; }
        public string Player { get; private set; }
        public TimeSpan Time { get; private set; }

        public ScoreGoal(Guid gameId, Team team, string player, TimeSpan time)
        {
            GameId = gameId;
            Team = team;
            Player = player;
            Time = time;
        }
    }
}