using LanguageExt;
using static LanguageExt.Prelude;
using static ProcessBus.Config.Errors.ConfigErrorConstructor;

namespace ProcessBus.Config.Errors
{
    public class ConfigErrorMap
    {
        public Map<ConfigError, ConfigErrorInfo> Mappings = Map(
            Tuple
            (
                ConfigError.CyclicConfiguration,
                ConfigError
                (
                    "Cyclic configuration error",
                    "Messaging topologies with cycles are not supported."
                )
            ),
            Tuple(
                ConfigError.DuplicateTransports,
                ConfigError
                (
                    "Duplicate transport error",
                    "Duplicate transports detected. Remove duplicate transports from configuration and try again."
                )
            ),
            Tuple
            (
                ConfigError.SelfForwards,
                ConfigError
                (
                    "Self forward error",
                    "Transports cannot forward to themselves."
                )
            )
        );

    }
}
