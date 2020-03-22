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


        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PluginCode;
        }
    }
}
