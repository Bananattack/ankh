using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ankh.framework.messages
{
    class Listener<T> where T : Topic
    {
        public Listener(Action<T> handler)
        {
            MessageCenter<T>.Register(handler);
        }
    }
}
