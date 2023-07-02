using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastConnect.Models.SQLite
{
    public class ConfigEntry
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public T GetValue<T>()
        {
            return (T)Convert.ChangeType(Value, typeof(T));
        }
    }
}
