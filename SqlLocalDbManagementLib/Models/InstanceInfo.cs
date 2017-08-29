using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlLocalDBManagementLib.Models
{
    public class InstanceInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string SharedName { get; set; }
        public string Owner { get; set; }
        public string Auto { get; set; }
        public string State { get; set; }
        public string LastStart { get; set; }
        public string PipeName { get; set; }
    }
}
