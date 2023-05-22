using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace StaticReflection
{
    public interface IAttributeDefine
    {
        IReadOnlyList<Attribute> Attributes { get; }
    }
    public interface IPropertyDefine<TInstance,TResult>: IAttributeDefine
    {
        Type DeclareType { get; }

        string PropertyName { get; }

        Type PropertyType { get; }

        bool CanRead { get; }

        bool CanWrite { get; }

        void SetValue(TInstance instance, TResult value);

        TResult GetValue(TInstance instance);
    }
}
