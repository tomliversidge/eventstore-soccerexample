using System;
using Domain.Infrastructure;

namespace Domain.Commands
{
    public class ShowPlayerRedCard : Command
    {
        public Guid GameId { get; private set; }
        public Team Team { get; private set; }
        public string PlayerName { get; private set; }
        public TimeSpan Time { get; private set; }
        
        public ShowPlayerRedCard(Guid gameId, Team team, string playerName, TimeSpan time)
        {
            GameId = gameId;
            Team = team;
            PlayerName = playerName;
            Time = time;
        }
    }
}