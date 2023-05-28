namespace StaticReflection
{
    public class WeakEventManager<TEventHandler>
        where TEventHandler:Delegate
    {
        private readonly IDictionary<TEventHandler, WeakReference> handlers;

        public WeakEventManager(IDictionary<TEventHandler, WeakReference> handlers)
        {
            this.handlers = handlers;
        }

        public WeakEventManager()
            :this(new  Dictionary<TEventHandler, WeakReference>())
        {
        }

        public IEnumerable<TEventHandler> Handlers => handlers.Keys;

        public void Subscribe(TEventHandler handler)
        {
            var weakReference = new WeakReference(handler.Target, trackResurrection: false);
            handlers[handler] = weakReference;
        }
        public bool UnSubscribe(TEventHandler handler)
        {
            return handlers.Remove(handler);
        }
        public void Clear()
        {
            handlers.Clear();
        }
    }
}
