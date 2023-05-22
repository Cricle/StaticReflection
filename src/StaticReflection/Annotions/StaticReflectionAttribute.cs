using System;
using System.Collections.Generic;
using System.Text;

namespace StaticReflection.Annotions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly | AttributeTargets.Property)]
    public class StaticReflectionAttribute : Attribute
    {
        public Type? Type { get; set; }
    }
}
