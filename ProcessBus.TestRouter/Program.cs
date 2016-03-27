using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Trans;
using ProcessBus.Config;
using ProcessBus.Config.Errors;
using static ProcessBus.Router.RouterFunctions;
using static LanguageExt.Process;

namespace ProcessBus.TestRouter
{
    class Program
    {
        static void Main(string[] args)
        {
            var configBuilder = new ConfigBuilder();
            var config = configBuilder
                .Transport("Email")
                    .Forward("Something")
                .Transport("Something")
                    .Forward("Else")
                .Transport("Else")
                    .Forward("Fish")
                    .Forward("Sodium")
                .Transport("Fish")
                    .Forward("Grapes")
                .Transport("Graphs")
                    .Forward("Fish")
                .Transport("Grapes")
                .Transport("Sodium")
                    .Forward("Eggs")
                .Transport("Eggs")
                .Build();

            var set = from someConfig in config
                from item in someConfig.Transports.Find(t => t.Path == "Graphs")
                select BfsTransport(someConfig, item);

            set.MapT(s =>
            {
                Console.WriteLine(s.Count);
                foreach (var forward in s)
                {
                    Console.WriteLine(forward.Path);
                }
                return Unit.Default;
            });

            Console.WriteLine();

            Console.ReadLine();

        }
    }
}
