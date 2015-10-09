using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshFruit.Common.DBUtils
{
    [Flags]
    public enum DatabaseMode
    {
        ReadOnly = 0x01,
        Write = 0x02,
        All = ReadOnly | Write
    }
}
