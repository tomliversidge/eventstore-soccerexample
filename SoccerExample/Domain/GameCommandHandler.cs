using System;
using Domain.Commands;
using Domain.Events;
using TomKernel;

namespace Domain
{
    public class GameCommandHandler : 
        IHandle<ScheduleGame>, 
        IHandle<KickOff>, 
        IHandle<ScoreGoal>,
        IHandle<BlowFullTimeWhistle>,
        IHandle<SubstitutePlayer>,
        IHandle<ShowPlayerYellowCard>,
        IHandle<ShowPlayerRedCard>
    {
        private readonly IRepository<Game> _gameRepository;

        public GameCommandHandler(IRepository<Game> gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public void Handle(ScheduleGame args)
        {
            var game = new Game(args.GameId, args.HomeTeam, args.AwayTeam, args.KickOffTime);
            _gameRepository.Save(game);
        }

        public void Handle(KickOff args)
        {
            var game = _gameRepository.GetById(args.GameId);
            game.KickOff(args.Time);
            _gameRepository.Save(game);
        }

        public void Handle(ScoreGoal args)
        {
            var game = _gameRepository.GetById(args.GameId);
            game.ScoreGoal(args.Team, args.Player, args.Time);
            _gameRepository.Save(game);
        }
        
        public void Handle(BlowFullTimeWhistle args)
        {
            var game = _gameRepository.GetById(args.GameId);
            game.FullTime(args.Time);
            _gameRepository.Save(game);
        }

        public void Handle(SubstitutePlayer args)
        {
            var game = _gameRepository.GetById(args.GameId);
            game.SubstitutePlayer(args.Team, args.PlayerToBringOn, args.PlayerToSubstitute, args.Reason, args.Time);
            _gameRepository.Save(game);
        }

        public void Handle(ShowPlayerYellowCard args)
        {
            var game = _gameRepository.GetById(args.GameId);
            game.ShowPlayerYellowCard(args.Team, args.PlayerName, args.Time);
            _gameRepository.Save(game);
        }

        public void Handle(ShowPlayerRedCard args)
        {
            var game = _gameRepository.GetById(args.GameId);
            game.ShowPlayerRedCard(args.Team, args.PlayerName, args.Time);
            _gameRepository.Save(game);
        }
    }
}