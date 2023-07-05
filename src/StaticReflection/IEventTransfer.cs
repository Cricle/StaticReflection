namespace StaticReflection
{
    public interface IEventBearing
    {
        event EventHandler<EventTransferEventArgs> EventTransfed;
    }
    public interface IEventTransfer: IEventBearing
    {
        bool IsListening { get; }

        bool Start(object? instance);

        bool Stop(object? instance);

        IEventTransferScope CreateScope(object? instance);
    }

    public interface IEventTransferScope: IEventBearing,IDisposable
    {
        IEventTransfer Root { get; }

        bool IsListening { get; }

        object? Instance { get; }

        bool Start();

        bool Stop();
    }
}
