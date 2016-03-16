using System;

namespace ProcessBus
{
    public class ProcessBusMessage
    {
        public ProcessBusMessage(string path, Type messageType, string serializedMessage)
        {
            Path = path;
            MessageType = messageType;
            SerializedMessage = serializedMessage;
        }

        public Type MessageType { get; }
        public string Path { get; }
        public string SerializedMessage { get; }
    }
}
