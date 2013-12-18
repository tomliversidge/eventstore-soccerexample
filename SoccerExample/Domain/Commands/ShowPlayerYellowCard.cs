using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Infrastructure;

namespace Domain.Commands
{
    public class ShowPlayerYellowCard : Command
    {
        public Guid GameId { get; private set; }
        public Team Team { get; private set; }
        public string PlayerName { get; private set; }
        public TimeSpan Time { get; private set; }

        public ShowPlayerYellowCard(Guid gameId, Team team, string playerName, TimeSpan time)
        {
            GameId = gameId;
            Team = team;
            PlayerName = playerName;
            Time = time;
        }
    }
}
