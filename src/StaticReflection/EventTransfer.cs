namespace StaticReflection
{
    public abstract class EventTransfer : IEventTransfer
    {
        private readonly WeakEventManager<EventHandler<EventTransferEventArgs>> eventTransferWeakEventManager;

        protected EventTransfer()
        {
            eventTransferWeakEventManager = new WeakEventManager<EventHandler<EventTransferEventArgs>>();
        }

        private long isListening;

        public bool IsListening => Interlocked.Read(ref isListening) != 0;

        public event EventHandler<EventTransferEventArgs> EventTransfed
        {
            add
            {
                eventTransferWeakEventManager.Subscribe(value);
            }

            remove
            {
                eventTransferWeakEventManager.UnSubscribe(value);
            }
        }

        public bool Start(object instance)
        {
            if (Interlocked.CompareExchange(ref isListening, 1, 0) == 1)
            {

                return true;
            }
            return false;
        }
        protected abstract void OnStart(object instance);
        public bool Stop(object instance)
        {
            if (Interlocked.CompareExchange(ref isListening, 0, 1) == 0)
            {

                return true;
            }
            return false;
        }
        protected abstract void OnStop(object instance);

        public void OnEventTransfed(EventTransferEventArgs args)
        {
            foreach (var item in eventTransferWeakEventManager.Handlers)
            {
                item(this, args);
            }
        }
    }
}
