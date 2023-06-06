namespace StaticReflection
{
    public interface IPropertyAnonymousInvokeDefine
    {
        void SetValueAnonymous(object? instance, object? value);

        object GetValueAnonymous(object? instance);
    }
    public interface IPropertyInvokeDefine<TInstance, TResult>
    {
        void SetValue(TInstance instance, TResult value);

        TResult GetValue(TInstance instance);
    }
    public interface IPropertyDefine : IAttributeDefine, IMemberDefine
    {    
        Type PropertyType { get; }

        bool CanRead { get; }

        bool CanWrite { get; }

        bool IsRequired { get; }

        bool IsWithEvents { get; }

        bool ReturnsByRef { get; }

        bool ReturnsByRefReadonly { get; }

        IReadOnlyList<Attribute> GetterAttributes { get; }

        IReadOnlyList<Attribute> SetterAttributes { get; }
    }
}
