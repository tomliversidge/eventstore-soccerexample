using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Infrastructure
{
    public interface ICommandSender
    {
        void Send<T>(T command) where T : ICommand;

    }
    public interface IEventPublisher
    {
        void Publish<T>(T @event) where T : Event;
    }
    public static class MessageDispatcher
    {
        [ThreadStatic] // so the thread has its own callbacks
        private static List<Delegate> actions;

        public static void Register<T>(Action<T> callback) where T : IMessage
        {
            if (actions == null)
                actions = new List<Delegate>();
            actions.Add(callback);
        }

        public static void ClearCallbacks()
        {
            actions = null;
        }
        
        /// <summary>
        /// Sends a command
        /// </summary>
        public static void Send<T>(T args) where T : ICommand
        {
            if (actions != null)
            {
                foreach (var action in actions)
                {
                    if (action is Action<T>)
                    {
                        ((Action<T>)action)(args);
                    }
                }
            }
        }

        /// <summary>
        /// Publishes a domain event
        /// </summary>
        public static void Publish<T>(T args) where T : IEvent
        {
            if (actions != null)
            {
                foreach (var action in actions)
                {
                    if (action is Action<T>)
                    {
                        ((Action<T>)action)(args);
                    }
                }
            }
        }
    }
}