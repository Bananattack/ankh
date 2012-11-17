using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ankh.framework.messages
{
    static class MessageCenter<T> where T : ITopic
    {
        //---------------------------------------------------------------------
        public static void Publish(T topic)
        {
            foreach(var maybeHandler in handlers)
            {
                if (maybeHandler.IsAlive)
                {
					Action<T> listener = maybeHandler.Target as Action<T>;
					listener.Invoke(topic);
                }
                else
                {
                    throw new NullReferenceException("Found a null listener. Did you forget to dispose it?");
                }
            }
        }

        //---------------------------------------------------------------------
        public static void Clear()
        {
            handlers.Clear();
        }

        //---------------------------------------------------------------------
        internal static void Register(Action<T> handler)
        {
			handlers.Add(new WeakReference(handler));
        }

        //---------------------------------------------------------------------
        internal static void Unregister(Action<T> handler)
        {
            handlers.RemoveAll(weakRef =>
            {
                return (weakRef.Target == handler);
            });
            
        }

        //---------------------------------------------------------------------
        // Private members
        //---------------------------------------------------------------------
        static List<WeakReference> handlers = new List<WeakReference>();

        
    }
}
