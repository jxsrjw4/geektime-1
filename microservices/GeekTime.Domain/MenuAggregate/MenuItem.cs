using System;
using System.Collections.Generic;
using System.Text;

namespace GeekTime.Domain.MenuAggregate
{
    public class MenuItem:ValueObject
    {
        public string PluginName { get; private set; }

        public string PluginCode { get; private set; }

        public string PluginDllName { get; private set; }

        public MenuItem() { }

        public MenuItem(string pluginName, string pluginCode, string pluginDllName)
        {
            this.PluginName = pluginName;
            this.PluginCode = pluginCode;
            this.PluginDllName = pluginDllName;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PluginCode;
        }
    }
}
