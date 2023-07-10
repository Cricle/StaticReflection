namespace StaticReflection
{
    public interface IUnderType
    {
        Type DeclareType { get; }

        string Name { get; }
    }
    public interface IMemberDefine : IAttributeDefine, IUnderType
    {
        string MetadataName { get; }

        bool IsVirtual { get; }

        bool IsStatic { get; }

        bool IsOverride { get; }

        bool IsAbstract { get; }

        bool IsSealed { get; }

        bool IsDefinition { get; }

        bool IsExtern { get; }

        bool IsImplicitlyDeclared { get; }

        bool CanBeReferencedByName { get; }

        bool IsPublic { get; }

        bool IsPrivate { get; }

        bool IsProtected { get; }

        bool IsInternal { get; }
    }
}
