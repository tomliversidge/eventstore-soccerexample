using System;
using Domain.Infrastructure;

namespace Domain.Events
{
    public class PlayerYellowCarded : Event
    {
        public Team Team { get; set; }
        public string PlayerName { get; set; }
        public TimeSpan Time { get; set; }

        public PlayerYellowCarded(Team team, string playerName, TimeSpan time)
        {
            Team = team;
            PlayerName = playerName;
            Time = time;
        }
    }
}