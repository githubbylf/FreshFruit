using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshFruit.Common.BaseCommon
{
    public class InvalidSystemClock : System.Exception
    {
        public InvalidSystemClock(string message) : base(message)
        {
        }
    }
}
