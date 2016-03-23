using System;

namespace ProcessBus.Config.Errors
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException(IConfigError error) : base($"{error.Name}: {error.Message}")
        {
            ConfigError = error;
        }

        public IConfigError ConfigError { get; }
    }
}
