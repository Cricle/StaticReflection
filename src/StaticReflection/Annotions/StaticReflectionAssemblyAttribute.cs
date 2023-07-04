﻿namespace StaticReflection.Annotions
{
    [AttributeUsage(AttributeTargets.Class,Inherited =false,AllowMultiple =false)]
    public sealed class StaticReflectionAssemblyAttribute : Attribute
    {
        public bool WithDefault { get; set; } = true;
    }
}
