using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshFruit.Common.BaseCommon
{
    public class DisposableAction : System.IDisposable
    {
        private readonly Action _action;

        public DisposableAction(Action action)
        {
            if (action == null)
            {
                throw new System.ArgumentNullException("action");
            }
            this._action = action;
        }

        public void Dispose()
        {
            this._action();
        }
    }
}
