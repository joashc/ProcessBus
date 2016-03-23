namespace ProcessBus.Config.Errors
{
    public class ParseError : IConfigError
    {
        public string Name { get; } = "Configuration parse error";
        public string Message { get; } = "Error parsing config.";
    }
}
