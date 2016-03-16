using LanguageExt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using static LanguageExt.Process;

namespace ProcessBus
{
    public static class Send
    {
        public static Unit send<T>(string transport, T message) where T : class
        {
            var ms = new MemoryStream();
            using (var writer = new BsonWriter(ms))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, message);
            }
            var serialized = Convert.ToBase64String(ms.ToArray());
            var pbm = new ProcessBusMessage(transport, message.GetType(), serialized);
            tell("@router", pbm);
            return Unit.Default;
        }
    }
}
