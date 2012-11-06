using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ankh.framework.messages
{
    static class MessageCenter<T> where T : Topic
    {
        //---------------------------------------------------------------------
        public static void Publish(T topic)
        {
            
            foreach(var maybeHandler in handlers)
            {
                Action<T> listener;
                if (maybeHandler.TryGetTarget(out listener))
                {
                    listener.Invoke(topic);
                }
                else
                {
                    invalidHandlers.Add(maybeHandler);
                }
            }

            //--Clean up invalids-----------------------
            foreach (var invalidHandler in invalidHandlers)
            {
                handlers.Remove(invalidHandler);
            }
            invalidHandlers.Clear();
        }

        //---------------------------------------------------------------------
        internal static void Register(Action<T> handler)
        {
            handlers.Add(new WeakReference<Action<T>>(handler));
        }


        //---------------------------------------------------------------------
        // Private members
        //---------------------------------------------------------------------
        static List<WeakReference<Action<T>>> handlers = new List<WeakReference<Action<T>>>();
        static List<WeakReference<Action<T>>> invalidHandlers = new List<WeakReference<Action<T>>>();
    }
}
