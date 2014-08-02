using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TomKernel.Exceptions;

namespace TomKernel
{
    public class EventStoreRepository<T> : IRepository<T> where T : AggregateRoot
    {
        private const string EventClrTypeHeader = "EventClrTypeName";
        private const string AggregateClrTypeHeader = "AggregateClrTypeName";
        private const string CommitIdHeader = "CommitId";
        private const int WritePageSize = 500;
        private const int ReadPageSize = 500;

        private readonly Func<Type, Guid, string> _aggregateIdToStreamName;

        private readonly IEventStoreConnection _eventStoreConnection;
        private static readonly JsonSerializerSettings SerializerSettings;

        static EventStoreRepository()
        {
            SerializerSettings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.None};
        }

        public EventStoreRepository(IEventStoreConnection eventStoreConnection)
            : this(
                eventStoreConnection,
                (t, g) => string.Format("{0}-{1}", char.ToLower(t.Name[0]) + t.Name.Substring(1), g.ToString("N")))
        {
        }

        public EventStoreRepository(IEventStoreConnection eventStoreConnection,
                                       Func<Type, Guid, string> aggregateIdToStreamName)
        {
            _eventStoreConnection = eventStoreConnection;
            _aggregateIdToStreamName = aggregateIdToStreamName;
        }

        private static TAggregate ConstructAggregate<TAggregate>()
        {
            return (TAggregate)Activator.CreateInstance(typeof(TAggregate), true);
        }

        private static object DeserializeEvent(byte[] metadata, byte[] data)
        {
            var eventClrTypeName = JObject.Parse(Encoding.UTF8.GetString(metadata)).Property(EventClrTypeHeader).Value;
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string) eventClrTypeName));
        }

        public static EventData ToEventData(Guid eventId, object evnt, IDictionary<string, object> headers)
        {
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evnt, SerializerSettings));

            var eventHeaders = new Dictionary<string, object>(headers)
            {
                {
                    EventClrTypeHeader, evnt.GetType().AssemblyQualifiedName
                },
                {"Test", "Testing"}
            };
            var metadata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventHeaders, SerializerSettings));
            var typeName = evnt.GetType().Name;

            return new EventData(eventId, typeName, true, data, metadata);
        }

        public T GetById(Guid id)
        {
            return GetById(id, int.MaxValue);
        }

        public T GetById(Guid id, int version)
        {
            if (version <= 0)
                throw new InvalidOperationException("Cannot get version <= 0");

            var streamName = _aggregateIdToStreamName(typeof(T), id);
            var aggregate = ConstructAggregate<T>();

            var sliceStart = 0;
            StreamEventsSlice currentSlice;
            do
            {
                var sliceCount = sliceStart + ReadPageSize <= version
                                     ? ReadPageSize
                                     : version - sliceStart + 1;

                currentSlice = _eventStoreConnection.ReadStreamEventsForward(streamName, sliceStart, sliceCount, false);

                if (currentSlice.Status == SliceReadStatus.StreamNotFound)
                    throw new AggregateNotFoundException(id, typeof(T));

                if (currentSlice.Status == SliceReadStatus.StreamDeleted)
                    throw new AggregateDeletedException(id, typeof(T));

                sliceStart = currentSlice.NextEventNumber;
                aggregate.LoadsFromHistory(currentSlice.Events.Select(evnt=> (Event)DeserializeEvent(evnt.OriginalEvent.Metadata, evnt.OriginalEvent.Data)));
            } while (version >= currentSlice.NextEventNumber && !currentSlice.IsEndOfStream);

            if (aggregate.Version != version && version < Int32.MaxValue)
                throw new AggregateVersionException(id, typeof(T), aggregate.Version, version);
               
            return aggregate;
        }
        
        public void Save(AggregateRoot aggregate)
        {  
            var commitHeaders = new Dictionary<string, object>
                {
                    {CommitIdHeader, Guid.NewGuid()},
                    {AggregateClrTypeHeader, aggregate.GetType().AssemblyQualifiedName}
                };
            
            var eventsToSave = aggregate.GetUncommittedChanges().Select(e => ToEventData(e.Id, e, commitHeaders)).ToList();
            var streamName = _aggregateIdToStreamName(aggregate.GetType(), aggregate.Id);
            var originalVersion = aggregate.Version - eventsToSave.Count;

            if (eventsToSave.Count < WritePageSize)
            {
                _eventStoreConnection.AppendToStream(streamName, originalVersion, eventsToSave);
            }
            else
            {
                var transaction = _eventStoreConnection.StartTransaction(streamName, originalVersion);

                var position = 0;
                while (position < eventsToSave.Count)
                {
                    var pageEvents = eventsToSave.Skip(position).Take(WritePageSize);
                    transaction.Write(pageEvents);
                    position += WritePageSize;
                }

                transaction.Commit();
            }

            aggregate.MarkChangesAsCommitted();
        }
    }
}