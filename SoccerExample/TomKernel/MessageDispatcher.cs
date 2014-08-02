using System;
using System.Collections.Generic;
using System.Linq;

namespace TomKernel
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

        public static IContainer Container { get; set; }

        // this method is used to enable testing
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
            if (Container != null)
            {
                var handlers = Container.ResolveAll<IHandle<T>>().ToList();
                if (handlers.Count == 0) throw new InvalidOperationException("no handler registered");
                if (handlers.Count > 1) throw new InvalidOperationException("cannot send to more than one handler");
                handlers[0].Handle(args);
            }
        
            // the following code is only used for testing when the Register method has been used
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
            if (Container != null)
            {
                foreach (var handler in Container.ResolveAll<IHandle<T>>())
                {
                    handler.Handle(args);
                }
            }

            // the following code is only used for testing when the Register method has been used
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