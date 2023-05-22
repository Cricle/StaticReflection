namespace StaticReflection
{
    public interface IMethodDefine : IMemberDefine
    {
        Type ReturnType { get; }

        IReadOnlyList<Type> ArgumentTypes { get; }

        bool IsGenericMethod { get; }

        int Arity { get; }

        bool IsExtensionMethod { get; }

        bool IsAsync { get; }

        bool IsVararg { get; }

        bool IsCheckedBuiltin { get; }

        bool HidesBaseMethodsByName { get; }

        bool ReturnsVoid { get; }

        bool ReturnsByRef { get; }

        bool ReturnsByRefReadonly { get; }

        bool IsReadOnly { get; }

        bool IsInitOnly { get; }

        bool IsPartialDefinition { get; }

        bool IsConditional { get; }

        IReadOnlyList<ITypeArgumentDefine> TypeArguments { get; }
    }
}
