namespace StaticReflection
{
    public interface IEventTransfer
    {
        event EventHandler<EventTransferEventArgs> EventTransfed;

        bool IsListening { get; }

        bool Start();

        bool Stop();
    }
}
