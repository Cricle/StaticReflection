namespace StaticReflection
{
    public interface ITypeDefine : IMemberDefine
    {
        int TypeKind { get; }

        Type? BaseType { get; }

        bool IsReferenceType { get; }

        bool IsValueType { get; }

        bool IsAnonymousType { get; }

        bool IsTupleType { get; }

        bool IsNativeIntegerType { get; }

        bool IsRefLikeType { get; }

        bool IsUnmanagedType { get; }

        bool IsReadOnly { get; }

        bool IsRecord { get; }

        StaticNullableAnnotation NullableAnnotation { get; }

        IReadOnlyList<string> Interfaces { get; }

        IReadOnlyList<string> AllInterfaces { get; }

        IReadOnlyList<IPropertyDefine> Properties { get; }

        IReadOnlyList<IMethodDefine> Methods { get; }

        IReadOnlyList<IEventDefine> Events { get; }

        IReadOnlyList<IConstructorDefine> Constructors { get; }
    }
}
