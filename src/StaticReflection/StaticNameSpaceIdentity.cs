namespace StaticReflection
{
    public class StaticNameSpaceIdentity : INameSpaceIdentity
    {
        public StaticNameSpaceIdentity(string name, bool isGlobalNamespace, StaticNamespaceKind namespaceKind)
        {
            Name = name;
            IsGlobalNamespace = isGlobalNamespace;
            NamespaceKind = namespaceKind;
        }

        public string Name { get; }

        public bool IsGlobalNamespace { get; }

        public StaticNamespaceKind NamespaceKind { get; }
    }
}
