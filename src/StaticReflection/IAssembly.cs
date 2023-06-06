namespace StaticReflection
{
    public interface IAssembly
    {
        string Name { get; }

        IReadOnlyList<ITypeDefine> Types { get; }
    }
}
