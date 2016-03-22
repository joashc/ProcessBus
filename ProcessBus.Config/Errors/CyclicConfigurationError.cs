namespace ProcessBus.Config.Errors
{
    public class CyclicConfigurationError : IConfigError
    {
        public string Name { get; } = "Cyclic configuration";
        public string Message { get; } = "Messaging topologies with cycles are not supported.";
    }
}
