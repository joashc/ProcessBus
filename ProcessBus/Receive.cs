using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using static LanguageExt.Prelude;

namespace ProcessBus
{
    public static class Receive
    {
        public static T ParseMessage<T>(ProcessBusMessage message) where T : class
        {
            var ms = new MemoryStream(Convert.FromBase64String(message.SerializedMessage));
            using (var reader = new BsonReader(ms))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<T>(reader);
            }
        }

        public static Action<ProcessBusMessage> CreateHandler<T>(Action<T> handler) where T : class
        {
            return message =>
            {
                var m = ParseMessage<T>(message);
                handler(m);
            };
        }

        public static Unit TryHandle(Map<Type, Action<ProcessBusMessage>> handlerMap, ProcessBusMessage message)
        {
            handlerMap.Find(message.MessageType)
                .IfSome(handler => handler(message));
            return Unit.Default;
        }

        public static SubscriptionBuilder Subscribe(string topic)
        {
            return new SubscriptionBuilder(topic);
        }

    }
}
