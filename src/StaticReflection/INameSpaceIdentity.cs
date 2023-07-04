namespace StaticReflection
{
    public interface INameSpaceIdentity
    {
        string Name { get; }

        bool IsGlobalNamespace { get; }

        StaticNamespaceKind NamespaceKind { get; }
    }
}
