using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using SqlLocalDBManagementLib.Models;

namespace SqlLocalDBManagementLib
{
    public class Basic
    {
        private IDictionary<ScriptEnum, String> scriptsDictionary;

        public Basic()
        {
            scriptsDictionary = new Dictionary<ScriptEnum, string>()
            {
                { ScriptEnum.FetchIntances, "FetchInstances" },
                { ScriptEnum.IntanceStatus, "InstanceStatus" },
                { ScriptEnum.SetState, "SetState" }
            };
        }

        public IList<string> GetInstancesNames()
        {
            var rows = ExecuteTextRows(ScriptEnum.FetchIntances);
            return rows.ToList();

        }

        public InstanceInfo GetInstanceStatus(string instanceName)
        {
            var rows = ExecuteTextRows(ScriptEnum.IntanceStatus, new { InstanceName = instanceName });
            return  RawToInstanceInfo(rows.ToList());
        }

        public bool Start(string instanceName)
        {
            var rows = ExecuteTextRows(ScriptEnum.SetState, new { State = "s", InstanceName = instanceName });
            var clean = CleanEmptyRows(rows);
            return (from c in clean where c.ToLower().Contains("started") select c).Any();
        }

        private InstanceInfo RawToInstanceInfo(IList<string> rows)
        {
            var clean = CleanEmptyRows(rows);

            InstanceInfo ii = new InstanceInfo();

            foreach (var row in clean)
            {
                var x = row.IndexOf(":");
                var first = row.Substring(0, x).ToLower();
                var second = row.Substring(++x).Trim();
                if (first.Equals("name"))
                {
                    ii.Name = second;
                }
                else if (first.Contains("version"))
                {
                    ii.Version = second;
                }
                else if (first.Contains("shared name"))
                {
                    ii.SharedName = second;
                }
                else if (first.Contains("owner"))
                {
                    ii.Owner = second;
                }
                else if (first.Contains("auto-create"))
                {
                    ii.Auto = second;
                }
                else if (first.Contains("state"))
                {
                    ii.State = second;
                }
                else if (first.Contains("last start"))
                {
                    ii.LastStart = second;
                }
                else if (first.Contains("pipe name"))
                {
                    ii.PipeName = second;
                }
            }

            return ii;

        }

        private IList<string> CleanEmptyRows(IList<string> rows)
        {
            return (from r in rows where !string.IsNullOrWhiteSpace(r) select r).ToList();
        }

        private Collection<string> ExecuteTextRows(ScriptEnum kind, Object @params = null)
        {
            using (PowerShell PSInst = PowerShell.Create())
            {
                var psText = TryReadScript(kind);
                PSInst.AddScript(psText);
                if (@params != null)
                {
                    var props = @params.GetType().GetProperties();
                    foreach (var property in props)
                    {
                        var val = property.GetValue(@params);
                        PSInst.AddParameter(property.Name, val);
                    }
                }
                Collection<string> POut = PSInst.Invoke<string>();
                return POut;
            }
        }

        private string TryReadScript(ScriptEnum kind)
        {
            try
            {
                return ReadScript(kind);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string ReadScript(ScriptEnum kind)
        {
            var shortname = scriptsDictionary[kind];
            var fullname = TryGetMappedScriptName(shortname);
            var script = TryReadResourceScript(fullname);
            return script;
        }

        private string TryGetMappedScriptName(string name)
        {
            try
            {
                return GetMappedScriptName(name);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetMappedScriptName(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resources = assembly.GetManifestResourceNames();
            var matches = from r in resources
                          where r.Contains(name)
                          select r;
            return matches.SingleOrDefault();

        }

        private string TryReadResourceScript(string resourceFullName)
        {
            try
            {
                return ReadResourceScript(resourceFullName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string ReadResourceScript(string resourceFullName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceFullName))
            using (StreamReader reader = new StreamReader(stream))
            {
                var script = reader.ReadToEnd();
                return script;
            }
        }
    }
}
