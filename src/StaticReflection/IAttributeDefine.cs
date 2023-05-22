namespace StaticReflection
{
    public interface IAttributeDefine
    {
        IReadOnlyList<Attribute> Attributes { get; }
    }
}
