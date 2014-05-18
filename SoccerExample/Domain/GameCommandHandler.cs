using System;
using Domain.Commands;
using Domain.Events;
using Domain.Infrastructure;

namespace Domain
{
    public static class GameCommandHandler
    {
        public static void Handle(IRepository<Game> gameRepository, ScheduleGame command)
        {
            var game = new Game(command.GameId, command.HomeTeam, command.AwayTeam, command.KickOffTime);
            gameRepository.Save(game);
        }

        public static void Handle(IRepository<Game> gameRepository, KickOff command)
        {
            var game = gameRepository.GetById(command.GameId);
            game.KickOff(command.Time);
            gameRepository.Save(game);
        }

        public static void Handle(IRepository<Game> gameRepository, ScoreGoal command)
        {
            var game = gameRepository.GetById(command.GameId);
            game.ScoreGoal(command.Team, command.Player, command.Time);
            gameRepository.Save(game);
        }

        public static void BlowFullTimeWhistle(IRepository<Game> gameRepository, BlowFullTimeWhistle command)
        {
            var game = gameRepository.GetById(command.GameId);
            game.FullTime(command.Time);
            gameRepository.Save(game);
        }

        public static void SubstitutePlayer(IRepository<Game> gameRepository, SubstitutePlayer command)
        {
            var game = gameRepository.GetById(command.GameId);
            game.SubstitutePlayer(command.Team, command.PlayerToBringOn, command.PlayerToSubstitute, command.Reason, command.Time);
            gameRepository.Save(game);
        }

        public static void ShowPlayerYellowCard(IRepository<Game> gameRepository, ShowPlayerYellowCard command)
        {
            var game = gameRepository.GetById(command.GameId);
            game.ShowPlayerYellowCard(command.Team, command.PlayerName, command.Time);
            gameRepository.Save(game);
        }

        public static void ShowPlayerRedCard(IRepository<Game> gameRepository, ShowPlayerRedCard command)
        {
            var game = gameRepository.GetById(command.GameId);
            game.ShowPlayerRedCard(command.Team, command.PlayerName, command.Time);
            gameRepository.Save(game);
        }
    }
}