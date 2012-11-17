using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ankh.framework.messages
{
    static class MessageCenter
    {
        //---------------------------------------------------------------------
        public static void Publish<T>(T topic) where T : ITopic
        {
            List<WeakReference> typeHandlers;
            if(handlers.TryGetValue(typeof(T), out typeHandlers))
            {
                foreach(var maybeHandler in typeHandlers)
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
        }

        //---------------------------------------------------------------------
        public static void Clear()
        {
            handlers.Clear();
        }

        //---------------------------------------------------------------------
        internal static void Register<T>(Action<T> handler) where T : ITopic
        {
            List<WeakReference> typeHandlers;
            if (handlers.TryGetValue(typeof(T), out typeHandlers))
            {
                typeHandlers.Add(new WeakReference(handler));
            }
            else
            {
                typeHandlers = new List<WeakReference> { new WeakReference(handler) };
                handlers.Add(typeof(T), typeHandlers);
            }
        }

        //---------------------------------------------------------------------
        internal static void Unregister<T>(Action<T> handler) where T : ITopic
        {
            handlers[typeof(T)].RemoveAll(weakRef =>
            {
                return (weakRef.Target == handler);
            });
            
        }

        //---------------------------------------------------------------------
        // Private members
        //---------------------------------------------------------------------
        static Dictionary<Type, List<WeakReference>> handlers = new Dictionary<Type, List<WeakReference>>();
    }
}
