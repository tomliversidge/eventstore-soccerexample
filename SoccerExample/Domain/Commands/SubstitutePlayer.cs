using System;
using TomKernel;

namespace Domain.Commands
{
    public class SubstitutePlayer : Command
    {
        public Guid GameId { get; private set; }
        public Team Team { get; set; }
        public string PlayerToSubstitute { get; private set; }
        public string PlayerToBringOn { get; set; }
        public SubstitutionReason Reason { get; private set; }
        public TimeSpan Time { get; private set; }

        public SubstitutePlayer(Guid gameId, Team team, string playerToSubstitute, string playerToBringOn, SubstitutionReason reason, TimeSpan time)
        {
            GameId = gameId;
            Team = team;
            PlayerToSubstitute = playerToSubstitute;
            PlayerToBringOn = playerToBringOn;
            Reason = reason;
            Time = time;
        }
    }
}