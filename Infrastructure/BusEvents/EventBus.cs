using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.BusEvents
{
    public static class EventBus
    {
        //todo optimize
        //todo write unit tests

        private static Dictionary<Type, List<IGlobalSubscriber>> _subscribers
            = new Dictionary<Type, List<IGlobalSubscriber>>();

        public static void RaiseEvent<TSubscriber>(Action<TSubscriber> action)
            where TSubscriber : IGlobalSubscriber
        {
            if (_subscribers.ContainsKey(typeof(TSubscriber)) == false)
                return;

            List<IGlobalSubscriber> subscribers = _subscribers[typeof(TSubscriber)].ToList();
            foreach (IGlobalSubscriber subscriber in subscribers)
            {
                action.Invoke((TSubscriber) subscriber);
            }
        }

        public static async Task RaiseEventAsync<TSubscriber>(Func<TSubscriber, Task> action)
            where TSubscriber : IGlobalSubscriber
        {
            List<IGlobalSubscriber> subscribers = _subscribers[typeof(TSubscriber)];
            foreach (IGlobalSubscriber subscriber in subscribers)
            {
                await action.Invoke((TSubscriber) subscriber);
            }
        }

        public static void Subscribe(IGlobalSubscriber subscriber)
        {
            List<Type> subscriberTypes = GetSubscribersTypes(subscriber);

            foreach (Type t in subscriberTypes)
            {
                if (!_subscribers.ContainsKey(t))
                    _subscribers[t] = new List<IGlobalSubscriber>();
                _subscribers[t].Add(subscriber);
            }
        }

        public static void Unsubscribe(IGlobalSubscriber subscriber)
        {
            List<Type> subscriberTypes = GetSubscribersTypes(subscriber);

            foreach (Type t in subscriberTypes)
            {
                if (_subscribers.ContainsKey(t))
                    _subscribers[t].Remove(subscriber);
            }
        }

        public static List<Type> GetSubscribersTypes(IGlobalSubscriber globalSubscriber)
        {
            Type type = globalSubscriber.GetType();

            //todo rewrite finding types
            List<Type> subscriberTypes = type
                .GetInterfaces()
                .Where(it =>
                    typeof(IGlobalSubscriber).IsAssignableFrom(it) &&
                    it != typeof(IGlobalSubscriber))
                .ToList();
            return subscriberTypes;
        }

        public static void Clear()
        {
            _subscribers.Clear();
        }
    }
}