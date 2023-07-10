namespace StaticReflection
{
    public interface ITypeArgumentDefine : IUnderType
    {
        bool HasReferenceTypeConstraint { get; }

        bool HasValueTypeConstraint { get; }

        bool HasUnmanagedTypeConstraint { get; }

        bool HasNotNullConstraint { get; }

        bool HasConstructorConstraint { get; }

        IReadOnlyList<Type> ConstraintTypes { get; }
    }
}
