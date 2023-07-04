namespace StaticReflection
{
    public interface ITypeDefine : IMemberDefine
    {
        IReadOnlyList<IPropertyDefine> Properties { get; }

        IReadOnlyList<IMethodDefine> Methods { get; }

        IReadOnlyList<IEventDefine> Events { get; }
    }
}
