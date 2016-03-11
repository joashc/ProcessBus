namespace ProcessBus.Config.Errors
{
    public static class ConfigErrorConstructor
    {
        public static ConfigErrorInfo ConfigError(string name, string message)
        {
            return new ConfigErrorInfo(name, message);
        }
        
    }

    public class ConfigErrorInfo
    {
        public ConfigErrorInfo(string name, string message)
        {
            Name = name;
            Message = message;
        }

        public string Name { get; }
        public string Message { get; }
    }
}
