using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Commands;
using Domain.Infrastructure;
using EventStore.ClientAPI;
using Persistence;

namespace ConsoleRunner
{
    class Program
    {
        private static void Log<T>(T message, Action<T> next) where T : IMessage
        {
            Console.WriteLine(message.ToString());
            next(message);
        }

        private static void RegisterWithLogging<T>(Action<T> callback) where T : IMessage
        {
            MessageDispatcher.Register<T>(x => Log(x, callback));
        }

        private static IRepository<Game> createGameRepository(IEventStoreConnection connection)
        {
            return new EventStoreRepository<Game>(connection);
        } 

        static void Main(string[] args)
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Loopback, 1113);

            var connection = EventStoreConnection.Create(ipEndPoint);
            connection.Connect();

            var gameRepository = new EventStoreRepository<Game>(connection);
            Func<IRepository<Game>> createGameRepository = () => new EventStoreRepository<Game>(connection);
            MessageDispatcher.Register<ScheduleGame>(x => Log(x, y => GameCommandHandler.Handle(gameRepository, x)));
            RegisterWithLogging<KickOff>(x => GameCommandHandler.Handle(gameRepository, x));
            MessageDispatcher.Register<ScoreGoal>(x => GameCommandHandler.Handle(gameRepository, x));
            MessageDispatcher.Register<BlowFullTimeWhistle>(x => GameCommandHandler.BlowFullTimeWhistle(gameRepository, x));
            MessageDispatcher.Register<SubstitutePlayer>(x => GameCommandHandler.SubstitutePlayer(gameRepository, x));
            MessageDispatcher.Register<ShowPlayerYellowCard>(x => GameCommandHandler.ShowPlayerYellowCard(gameRepository, x));
            // transient lifestyle 
            MessageDispatcher.Register<ShowPlayerRedCard>(x => GameCommandHandler.ShowPlayerRedCard(createGameRepository(), x));

            Console.WriteLine("Press any key to start");
            Console.ReadKey(); 
            Console.WriteLine();
            Console.WriteLine("Running games...");
            var game1Id = Guid.NewGuid();
            var game2Id = Guid.NewGuid();
            MessageDispatcher.Send(new ScheduleGame(game1Id, Team.ManUtd, Team.Arsenal, DateTime.Now));
            MessageDispatcher.Send(new KickOff(game1Id, DateTime.Now));
            MessageDispatcher.Send(new ScoreGoal(game1Id, Team.ManUtd, "Rooney", new TimeSpan(0, 10, 0)));
            MessageDispatcher.Send(new ScoreGoal(game1Id, Team.ManUtd, "Rooney", new TimeSpan(0, 23, 13)));
            MessageDispatcher.Send(new ShowPlayerYellowCard(game1Id, Team.Arsenal, "Giroud", new TimeSpan(0, 26, 13)));
            MessageDispatcher.Send(new ScoreGoal(game1Id, Team.ManUtd, "Rooney", new TimeSpan(0, 43, 13)));
            MessageDispatcher.Send(new ShowPlayerYellowCard(game1Id, Team.Arsenal, "Rooney", new TimeSpan(0, 63, 13)));
            MessageDispatcher.Send(new ScoreGoal(game1Id, Team.Arsenal, "Giroud", new TimeSpan(0, 76, 13)));
            MessageDispatcher.Send(new ShowPlayerRedCard(game1Id, Team.Arsenal, "Ramsey", new TimeSpan(0, 89, 13)));
            MessageDispatcher.Send(new BlowFullTimeWhistle(game1Id, DateTime.Now));

            MessageDispatcher.Send(new ScheduleGame(game2Id, Team.ManCity, Team.Chelsea, DateTime.Now));
            MessageDispatcher.Send(new KickOff(game2Id, DateTime.Now));
            MessageDispatcher.Send(new ScoreGoal(game2Id, Team.Chelsea, "Torres", new TimeSpan(0, 10, 0)));
            MessageDispatcher.Send(new ScoreGoal(game2Id, Team.ManCity, "Aguero", new TimeSpan(0, 23, 13)));
            MessageDispatcher.Send(new ShowPlayerYellowCard(game2Id, Team.Chelsea, "Cole", new TimeSpan(0, 26, 13)));
            MessageDispatcher.Send(new BlowFullTimeWhistle(game2Id, DateTime.Now));
            Console.WriteLine("done. Go to http://127.0.0.1:2113/web/streams.htm to see results. un: admin pw: changeit");
            Console.ReadKey();

        }
    }
}
