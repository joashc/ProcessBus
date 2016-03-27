using System.Runtime.InteropServices;
using LanguageExt;
using Newtonsoft.Json;
using static LanguageExt.Process;

namespace ProcessBus
{
    public static class SendMessage
    {
        public static Unit Send<T>(string transport, T message) where T : class
        {
            var serialized = JsonConvert.SerializeObject(message);
            var pbm = new ProcessBusMessage(transport, message.GetType(), serialized);
            var leastBusyRouter = Dispatch.leastBusy(find("router"));
            tell(leastBusyRouter, pbm);
            return Unit.Default;
        }
    }
}
