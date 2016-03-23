using System;

namespace ProcessBus.Config.Errors
{
    public class NonExistentTransportError : IConfigError
    {
        public NonExistentTransportError(Tuple<string, string> forward, bool toExists, bool fromExists)
        {
            Forward = forward;
            ToExists = toExists;
            FromExists = fromExists;
        }

        public string Name { get; } = "Non-existent forward";
        public string Message {
            get
            {
                if (!ToExists && !FromExists)
                    return
                        $"Configuration specifies forward between non-existent transports \"{Forward.Item1}\" and \"{Forward.Item2}\".";
                if (!ToExists)
                    return $"Configuration specifies forward from non-existent transport \"{Forward.Item1}\".";
                return $"Configuration specifies forward to non-existent transport \"{Forward.Item2}\".";
            }
        }
        public Tuple<string, string> Forward { get; }
        public bool ToExists { get; }
        public bool FromExists { get; } 
    }
}
