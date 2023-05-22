namespace StaticReflection
{
    public class TypeArgumentDefine : ITypeArgumentDefine
    {
        public TypeArgumentDefine(string name, Type declareType, bool hasReferenceTypeConstraint, bool hasValueTypeConstraint, bool hasUnmanagedTypeConstraint, bool hasNotNullConstraint, bool hasConstructorConstraint, IReadOnlyList<Type> constraintTypes)
        {
            Name = name;
            DeclareType = declareType;
            HasReferenceTypeConstraint = hasReferenceTypeConstraint;
            HasValueTypeConstraint = hasValueTypeConstraint;
            HasUnmanagedTypeConstraint = hasUnmanagedTypeConstraint;
            HasNotNullConstraint = hasNotNullConstraint;
            HasConstructorConstraint = hasConstructorConstraint;
            ConstraintTypes = constraintTypes;
        }

        public string Name { get; }

        public Type DeclareType { get; }

        public bool HasReferenceTypeConstraint { get; }

        public bool HasValueTypeConstraint { get; }

        public bool HasUnmanagedTypeConstraint { get; }

        public bool HasNotNullConstraint { get; }

        public bool HasConstructorConstraint { get; }

        public IReadOnlyList<Type> ConstraintTypes { get; }
    }
}
