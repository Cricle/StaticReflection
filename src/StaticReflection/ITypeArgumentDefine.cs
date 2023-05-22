namespace StaticReflection
{
    public interface IEventDefine : IMemberDefine
    {
        Type Type { get; }

        bool IsWindowsRuntimeEvent { get; }

        bool HasAddMethod { get; }

        bool HasRemoveMethod { get; }

        bool HasRaiseMethod { get; }

        bool IsOverriddenEvent { get; }
    }
    public interface ITypeArgumentDefine: IUnderType
    {
        bool HasReferenceTypeConstraint { get; }

        bool HasValueTypeConstraint { get; }

        bool HasUnmanagedTypeConstraint { get; }

        bool HasNotNullConstraint { get; }

        bool HasConstructorConstraint { get; }

        IReadOnlyList<Type> ConstraintTypes { get; }
    }
}
