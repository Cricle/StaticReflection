namespace StaticReflection
{
    public abstract class EventTransferScope : IEventTransferScope
    {
        protected readonly object? instance;
        private long isListening;

        protected readonly List<EventHandler<EventTransferEventArgs>> eventHandlers = new List<EventHandler<EventTransferEventArgs>>();

        public IReadOnlyList<EventHandler<EventTransferEventArgs>> EventHandlers => eventHandlers;

        public event EventHandler<EventTransferEventArgs> EventTransfed
        {
            add { eventHandlers.Add(value); }
            remove { eventHandlers.Remove(value); }
        }

        protected EventTransferScope(IEventTransfer root, object? instance)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
            this.instance = instance;
        }

        public bool IsListening => Interlocked.Read(ref isListening) != 0;

        public IEventTransfer Root { get; }

        public object? Instance => instance;

        public void Dispose()
        {
            Stop();
            OnDispose();
        }

        protected virtual void OnDispose()
        {

        }

        public bool Start()
        {
            if (Interlocked.CompareExchange(ref isListening, 1, 0) == 0)
            {
                OnStart();
                return true;
            }
            return false;
        }
        protected abstract void OnStart();
        public bool Stop()
        {
            if (Interlocked.CompareExchange(ref isListening, 0, 1) == 1)
            {
                OnStop();
                return true;
            }
            return false;
        }
        protected abstract void OnStop();

        public void OnEventTransfed(EventTransferEventArgs args)
        {
            foreach (var item in eventHandlers)
            {
                item.Invoke(this, args);
            }
        }
    }
    public abstract class EventTransfer : IEventTransfer
    {
        protected readonly List<EventHandler<EventTransferEventArgs>> eventHandlers = new List<EventHandler<EventTransferEventArgs>>();

        private long isListening;

        public bool IsListening => Interlocked.Read(ref isListening) != 0;

        public event EventHandler<EventTransferEventArgs> EventTransfed
        {
            add { eventHandlers.Add(value); }
            remove { eventHandlers.Remove(value); }
        }

        public bool Start(object? instance)
        {
            if (Interlocked.CompareExchange(ref isListening, 1, 0) == 0)
            {
                OnStart(instance);
                return true;
            }
            return false;
        }
        protected abstract void OnStart(object? instance);
        public bool Stop(object? instance)
        {
            if (Interlocked.CompareExchange(ref isListening, 0, 1) == 1)
            {
                OnStop(instance);
                return true;
            }
            return false;
        }
        protected abstract void OnStop(object? instance);

        public void OnEventTransfed(EventTransferEventArgs args)
        {
            foreach (var item in eventHandlers)
            {
                item.Invoke(this, args);
            }
        }

        public abstract IEventTransferScope CreateScope(object? instance);
    }
}
