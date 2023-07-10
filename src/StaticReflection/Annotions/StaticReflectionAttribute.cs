namespace StaticReflection.Annotions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class StaticReflectionAttribute : Attribute
    {
        public Type? Type { get; set; }
    }
}
