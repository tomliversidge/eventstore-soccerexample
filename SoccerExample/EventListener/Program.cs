using System;
using System.Net;
using System.Text;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EventListener
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = ConnectionSettings.Create(); 
            settings.SetDefaultUserCredentials(new UserCredentials("admin", "changeit"));
            var connection = EventStoreConnection.Create(settings, new IPEndPoint(IPAddress.Loopback, 1113));
            connection.Connect();
            //connection.SubscribeToStreamFrom("game-c4340792a0a64c5c9257cad75375ad36", StreamPosition.Start, true, GameEventAppeared);
            connection.SubscribeToStreamFrom("$ce-game", StreamPosition.Start, true, GameEventAppeared);
            Console.ReadLine();
        }

        private static void GameEventAppeared(EventStoreCatchUpSubscription arg1, ResolvedEvent resolvedEvent)
        {
            var originalMetaJson = JObject.Parse(Encoding.UTF8.GetString(resolvedEvent.Event.Metadata));
            var orignalData = Encoding.UTF8.GetString(resolvedEvent.Event.Data);
            var receivedEvent = resolvedEvent.Event;
            Console.WriteLine("{0:D4} - {1}", receivedEvent.EventNumber, receivedEvent.EventType);
            var deserializedEvent = DeserializeEvent(receivedEvent.Metadata, receivedEvent.Data);

        }

        private const string EventClrTypeHeader = "EventClrTypeName";
        private static object DeserializeEvent(byte[] metadata, byte[] data)
        {
            var metaJson = JObject.Parse(Encoding.UTF8.GetString(metadata));

            var eventClrTypeName = JObject.Parse(Encoding.UTF8.GetString(metadata)).Property(EventClrTypeHeader).Value;
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string)eventClrTypeName));
        }

    }
}
