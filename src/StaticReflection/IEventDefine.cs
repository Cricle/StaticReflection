using System;

namespace StaticReflection
{
    public class EventTransferEventArgs : EventArgs
    {
        public EventTransferEventArgs(IReadOnlyList<object> args)
        {
            Args = args;
        }

        public IReadOnlyList<object> Args { get; }
    }
    public abstract class EventTransfer : IEventTransfer
    {
        private readonly WeakEventManager<EventHandler<EventTransferEventArgs>> eventTransferWeakEventManager;

        protected EventTransfer()
        {
            eventTransferWeakEventManager = new WeakEventManager<EventHandler<EventTransferEventArgs>>();
        }

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
    }

    public interface IEventTransfer
    {
        event EventHandler<EventTransferEventArgs> EventTransfed;
    }
    public interface IEventDefine : IMemberDefine
    {
        Type DelegateType { get; }

        bool IsWindowsRuntimeEvent { get; }

        bool HasAddMethod { get; }

        bool HasRemoveMethod { get; }

        bool HasRaiseMethod { get; }

        bool IsOverriddenEvent { get; }

        IReadOnlyList<Type> ArgumentTypes { get; }
    }
}
