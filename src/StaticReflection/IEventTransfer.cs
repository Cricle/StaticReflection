namespace StaticReflection
{
    public interface IEventTransfer
    {
        event EventHandler<EventTransferEventArgs> EventTransfed;

        bool IsListening { get; }

        bool Start(object instance);

        bool Stop(object instance);
    }
}
