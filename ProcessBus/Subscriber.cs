using System;
using LanguageExt;
using Newtonsoft.Json;
using static LanguageExt.Prelude;

namespace ProcessBus
{
    public static class Subscriber
    {
        public static T ParseMessage<T>(ProcessBusMessage message) where T : class
        {
            return JsonConvert.DeserializeObject<T>(message.SerializedMessage);
        }

        public static Action<ProcessBusMessage> CreateHandler<T>(Action<T> handler) where T : class
        {
            return message =>
            {
                T parsed = ParseMessage<T>(message);
                handler(parsed);
            };
        }

        public static Unit TryHandle(Map<Type, Action<ProcessBusMessage>> handlerMap, ProcessBusMessage message)
        {
            handlerMap.Find(message.MessageType).IfSome(handler => handler(message));
            return Unit.Default;
        }

        public static SubscriptionBuilder Subscribe(string topic)
        {
            return new SubscriptionBuilder(topic);
        }

    }
}
