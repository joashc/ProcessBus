namespace ProcessBus.Config.Errors
{
    public class SelfForwardError : IConfigError
    {
        public SelfForwardError(string transport)
        {
            SelfForwardedTransport = transport;
        }

        public string Name { get; } = "Transport forwarded to self";
        public string Message => $"Transport \"{SelfForwardedTransport}\" is forwarded to itself.";
        public string SelfForwardedTransport { get; }
    }
}
