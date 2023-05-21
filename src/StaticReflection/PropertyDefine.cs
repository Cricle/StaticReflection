using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace StaticReflection
{
    public interface IPropertyDefine<TInstance,TResult>
    {
        Type DeclareType { get; }

        string PropertyName { get; }

        void SetValue(TInstance instance, TResult value);

        TResult GetValue(TInstance instance);
    }
}
