using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProcessBus.Config.Definitions;
using YamlDotNet.Serialization;
using static LanguageExt.Prelude;
using LanguageExt;
using ProcessBus.Config.Errors;

namespace ProcessBus.Config
{
    public static class YamlParser
    {
        public static Either<IConfigError, RoutingDefinition> ParseFromFile(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException("Config file does not exist.");
            using (var fileReader = File.OpenText(path))
            {
                return ParseFromReader(fileReader);
            }
        }
        public static Either<IConfigError, RoutingDefinition> ParseFromYamlString(string yaml)
        {
            using (var stringReader = new StringReader(yaml))
            {
                return ParseFromReader(stringReader);
            }
        }

        public static Either<IConfigError, RoutingDefinition> ParseFromReader(TextReader reader)
        {
            var deserializer = new Deserializer();
            var config = deserializer.Deserialize<SerializableDefinition>(reader);
            return ConfigParser.Parse(SerializableToRawConfig(config));
        }

        public static RawConfig SerializableToRawConfig(SerializableDefinition def)
        {
            var transports = List(def.Transports.Select(t => t.Path).ToArray());
            var forwardTo = def.Transports.SelectMany(t => t.ForwardTo.Select(f => Tuple(t.Path, f)));
            var forwardFrom = def.Transports.SelectMany(t => t.ForwardFrom.Select(f => Tuple(f, t.Path)));
            var forwards = List(forwardTo.Concat(forwardFrom).ToArray());
            return new RawConfig(transports, forwards);
        }
    }
}
