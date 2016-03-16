using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessBus.Config.Definitions;

namespace ProcessBus
{
    public class PBEnv
    {
        public string ConfigPath { get; set; }
        public string RouterPath { get; set; }
        public RoutingDefinition RoutingDefinition { get; set; }
    }
}