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
        static void Main(string[] args)
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Loopback, 1113);

            var connection = EventStoreConnection.Create(ipEndPoint);
            connection.Connect();

            var gameRepository = new EventStoreRepository<Game>(connection);

            // this could be done using a container
            var gameCommandHandler = new GameCommandHandler(gameRepository);
            MessageDispatcher.Register<ScheduleGame>(gameCommandHandler.Handle);
            MessageDispatcher.Register<KickOff>(gameCommandHandler.Handle);
            MessageDispatcher.Register<ScoreGoal>(gameCommandHandler.Handle);
            MessageDispatcher.Register<BlowFullTimeWhistle>(gameCommandHandler.Handle);
            MessageDispatcher.Register<SubstitutePlayer>(gameCommandHandler.Handle);
            MessageDispatcher.Register<ShowPlayerYellowCard>(gameCommandHandler.Handle);
            MessageDispatcher.Register<ShowPlayerRedCard>(gameCommandHandler.Handle);

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
