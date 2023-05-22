namespace StaticReflection
{
    public interface IEventDefine : IMemberDefine
    {
        Type DelegateType { get; }

        bool IsWindowsRuntimeEvent { get; }

        bool HasAddMethod { get; }

        bool HasRemoveMethod { get; }

        bool HasRaiseMethod { get; }

        bool IsOverriddenEvent { get; }

        IReadOnlyList<Type> ArgumentTypes { get; }
    }
}
