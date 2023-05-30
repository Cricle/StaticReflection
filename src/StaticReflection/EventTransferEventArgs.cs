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
}
