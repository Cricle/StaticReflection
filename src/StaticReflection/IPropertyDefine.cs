namespace StaticReflection
{
    public interface IPropertyDefine<TInstance, TResult> : IAttributeDefine, IMemberDefine
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

        void SetValue(TInstance instance, TResult value);

        TResult GetValue(TInstance instance);
    }
}
