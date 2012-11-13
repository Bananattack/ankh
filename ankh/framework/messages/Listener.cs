using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ankh.framework.messages
{
    class Listener<T> : IDisposable where T : Topic
    {
        private Action<T> callback;

        public Listener(Action<T> callback)
        {
            this.callback = callback;
            MessageCenter<T>.Register(callback);
        }

        public void Dispose()
        {
            MessageCenter<T>.Unregister(callback);
        }
    }
}
