using System;
using System.Collections.Generic;
using Domain.Events;
using Domain.Infrastructure;

namespace Domain
{
    public class Game : AggregateRoot
    {
        public Game()
        {
        }
        public Game(Guid gameId, Team homeTeam, Team awayTeam, DateTime kickOff)
        {
            ApplyChange(new GameScheduled(gameId, homeTeam, awayTeam, kickOff));
        }

        private Guid _id;
        public override Guid Id
        {
            get { return _id; }
        }
        public Team HomeTeam { get; private set; }
        public Team AwayTeam { get; private set; }
        public DateTime StartTime { get; set; }
        private int _homeTeamRemainingSubstitutions;
        private int _awayTeamRemainingSubstitutions;

        public Dictionary<Team, List<string>> Goals { get; set; }
        
        public void ScoreGoal(Team team, string player, TimeSpan timeOfGoal)
        {
            if(team != HomeTeam && team != AwayTeam) throw new ArgumentException(string.Format("{0} are not playing in this game", team));
            ApplyChange(new GoalScored(_id, team, player, timeOfGoal));
        }
        
        public void FullTime(DateTime time)
        {
            ApplyChange(new FullTime(_id, time));
        }

        public void KickOff(DateTime time)
        {
            ApplyChange(new KickedOff(_id, time));
        }

        public void SubstitutePlayer(Team team, string playerToBringOn, string playerToSubstitute, SubstitutionReason reason, TimeSpan time)
        {
            if ((team == HomeTeam && _homeTeamRemainingSubstitutions == 0) || (team == AwayTeam && _awayTeamRemainingSubstitutions == 0))
                throw new Exception("No substitutions remaining");
            ApplyChange(new PlayerSubstituted(team, playerToBringOn, playerToSubstitute, reason, time));
        }

        public void ShowPlayerYellowCard(Team team, string playerName, TimeSpan time)
        {
            ApplyChange(new PlayerYellowCarded(team, playerName, time));
        }

        public void ShowPlayerRedCard(Team team, string playerName, TimeSpan time)
        {
            ApplyChange(new PlayerRedCarded(team, playerName, time));
        }

        private void Apply(GameScheduled e)
        {
            _id = e.Id;
            _awayTeamRemainingSubstitutions = 3;
            _homeTeamRemainingSubstitutions = 3;
            HomeTeam = e.HomeTeam;
            AwayTeam = e.AwayTeam;
            Goals = new Dictionary<Team, List<string>>();
        }

        private void Apply(KickedOff e)
        {
            StartTime = e.StartedAt;
        }

        private void Apply(GoalScored e)
        {
            if (Goals.ContainsKey(e.Team))
                Goals[e.Team].Add(e.Player);
            else
                Goals.Add(e.Team, new List<string> { e.Player });
        }

        private void Apply(PlayerSubstituted e)
        {
            if (e.Team == HomeTeam)
            {
                _homeTeamRemainingSubstitutions--;
            }
            else if (e.Team == AwayTeam)
            {
                _awayTeamRemainingSubstitutions--;
            }
            //mark the player as having been subsituted
            //add the new player etc..
        }

        private void Apply(PlayerYellowCarded e)
        {
            // mark the player booked
            // some sort of business logic to remove any players receving their second yellow cards...
        }
    }
}