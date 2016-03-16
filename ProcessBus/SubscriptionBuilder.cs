using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Process;
using static ProcessBus.Receive;
using static LanguageExt.Prelude;
using static ProcessBus.Prelude;

namespace ProcessBus
{
    public class SubscriptionBuilder
    {
        private Map<Type, Action<ProcessBusMessage>> _handlerMap;
        private string Topic;

        public SubscriptionBuilder(string topic)
        {
            Topic = topic;
            _handlerMap = LanguageExt.Map<Type, Action<ProcessBusMessage>>.Empty;
        }

        public SubscriptionBuilder Handle<T>(Action<T> action) where T : class
        {
            var handler = CreateHandler(action);
            _handlerMap = _handlerMap.Add(typeof (T), handler);
            return this;
        }

        public ProcessId Spawn()
        {
            var process = spawn<ProcessBusMessage>($"handler-{Topic}-{ProcessSuffix()}", msg =>
            {
                TryHandle(_handlerMap, msg);
            });
            register(Topic, process);
            return process;
        }
    }
}
