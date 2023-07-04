namespace StaticReflection
{
    public interface IModuleIdentity : IMemberDefine
    {
        INameSpaceIdentity GlobalNamespace { get; }

        IReadOnlyList<IAssemblyIdentity> ReferencedAssemblies { get; }

        IReadOnlyList<string> ReferencedAssemblySymbolNames { get; }
    }
}
