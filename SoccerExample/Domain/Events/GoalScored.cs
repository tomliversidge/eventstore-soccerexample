using System;
using TomKernel;

namespace Domain.Events
{
    public class GoalScored : Event
    {
        public Guid GameId { get; private set; }
        public Team Team { get; private set; }
        public string Player { get; private set; }
        public TimeSpan Time { get; private set; }

        public GoalScored(Guid gameId, Team team, string player, TimeSpan time)
        {
            GameId = gameId;
            Team = team;
            Player = player;
            Time = time;
        }
    }
}