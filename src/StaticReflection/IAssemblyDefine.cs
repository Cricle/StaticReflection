namespace StaticReflection
{
    public interface IAssemblyDefine : IMemberDefine
    {
        string AssemblyFullName { get; }

        bool IsInteractive { get; }

        IAssemblyIdentity Identity { get; }

        INameSpaceIdentity GlobalNamespace { get; }

        IReadOnlyList<IModuleIdentity> Modules { get; }

        IReadOnlyList<string> TypeNames { get; }

        IReadOnlyList<string> NamespaceNames { get; }

        bool MightContainExtensionMethods { get; }

        IReadOnlyList<ITypeDefine> Types { get; }
    }
}
