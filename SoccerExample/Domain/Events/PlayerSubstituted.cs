using System;
using TomKernel;

namespace Domain.Events
{
    public class PlayerSubstituted : Event
    {
        public TimeSpan Time { get; set; }
        public SubstitutionReason Reason { get; set; }
        public string PlayerToSubstitute { get; set; }
        public string PlayerToBringOn { get; set; }
        public Team Team { get; set; }

        public PlayerSubstituted(Team team, string playerToBringOn, string playerToSubstitute, SubstitutionReason reason, TimeSpan time)
        {
            Team = team;
            PlayerToBringOn = playerToBringOn;
            PlayerToSubstitute = playerToSubstitute;
            Reason = reason;
            Time = time;
        }
    }
}