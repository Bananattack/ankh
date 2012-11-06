using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ankh.framework.messages
{
    class MessageCenter<T> where T : Topic
    {
        //---------------------------------------------------------------------
        public static void Publish(T topic)
        {
            foreach(var maybeListener in handlers)
            {
                Action<T> listener;
                if (maybeListener.TryGetTarget(out listener))
                {
                    listener.Invoke(topic);
                }
            }
        }

        //---------------------------------------------------------------------
        public static void Register(Action<T> handler)
        {
            handlers.Add(new WeakReference<Action<T>>(handler));
        }



        static List<WeakReference<Action<T>>> handlers = new List<WeakReference<Action<T>>>();
    }
}
