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
                if (maybeHandler.IsAlive)
                {
					Action<T> listener = maybeHandler.Target as Action<T>;
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
			handlers.Add(new WeakReference(handler));
        }


        //---------------------------------------------------------------------
        // Private members
        //---------------------------------------------------------------------
        static List<WeakReference> handlers = new List<WeakReference>();
        static List<WeakReference> invalidHandlers = new List<WeakReference>();
    }
}
