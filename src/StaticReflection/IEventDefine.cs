using System;

namespace StaticReflection
{
    public static class EventDefineExecuteExtensions
    {
        public static bool Start(this IEventDefine eventDefine,object instance)
        {
            return ((IEventTransfer)eventDefine).Start(instance);
        }
        public static bool Stop(this IEventDefine eventDefine, object instance)
        {
            return ((IEventTransfer)eventDefine).Stop(instance);
        }
    }

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
