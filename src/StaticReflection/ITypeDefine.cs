using StaticReflection.Invoking;
using System.Xml.Linq;

namespace StaticReflection
{
    public interface ITypeDefine : IMemberDefine
    {
        IReadOnlyList<IPropertyDefine> Properties { get; }

        IReadOnlyList<IMethodDefine> Methods { get; }

        IReadOnlyList<IEventDefine> Events { get; }
    }

    public static class TypeDefineExtensions
    {
        public static void SetProperty(this ITypeDefine typeDefine, object? instance, string name, object? value)
        {
            GetPropertyAnonymousInvoke(typeDefine, name).SetValueAnonymous(instance, value);
        }
        public static IPropertyAnonymousInvokeDefine GetPropertyAnonymousInvoke(this ITypeDefine typeDefine, string name)
        {
            var prop = typeDefine.Properties.FirstOrDefault(x => x.Name == name);
            if (prop == null)
            {
                throw new InvalidOperationException($"Type {typeDefine.Name} has not property {name}");
            }
            if (prop is not IPropertyAnonymousInvokeDefine anonymousInvokeDefine)
            {
                throw new InvalidOperationException($"Type {typeDefine.Name}.{name} must implement {typeof(IPropertyAnonymousInvokeDefine)}");
            }
            return anonymousInvokeDefine;
        }
        public static object? InvokeMethod(this ITypeDefine typeDefine, string name, object? instance, params object?[] args)
        {
            var prop = typeDefine.Methods.FirstOrDefault(x => x.Name == name);
            if (prop == null)
            {
                throw new InvalidOperationException($"Type {typeDefine.Name} has not method {name}");
            }
            if (prop.ReturnType == typeof(void))
            {
                switch (prop.ArgumentTypes.Count)
                {
                    case 0:
                        ((IVoidArgs0AnonymousMethod)prop).InvokeAnonymous(instance);
                        return null;
                    case 1:
                        ((IVoidArgs1AnonymousMethod)prop).InvokeAnonymous(instance, ref args[1]);
                        return null;
                    case 2:
                        ((IVoidArgs2AnonymousMethod)prop).InvokeAnonymous(instance, ref args[2], ref args[2]);
                        return null;
                    case 3:
                        ((IVoidArgs3AnonymousMethod)prop).InvokeAnonymous(instance, ref args[3], ref args[3], ref args[3]);
                        return null;
                    case 4:
                        ((IVoidArgs4AnonymousMethod)prop).InvokeAnonymous(instance, ref args[4], ref args[4], ref args[4], ref args[4]);
                        return null;
                    case 5:
                        ((IVoidArgs5AnonymousMethod)prop).InvokeAnonymous(instance, ref args[5], ref args[5], ref args[5], ref args[5], ref args[5]);
                        return null;
                    case 6:
                        ((IVoidArgs6AnonymousMethod)prop).InvokeAnonymous(instance, ref args[6], ref args[6], ref args[6], ref args[6], ref args[6], ref args[6]);
                        return null;
                    case 7:
                        ((IVoidArgs7AnonymousMethod)prop).InvokeAnonymous(instance, ref args[7], ref args[7], ref args[7], ref args[7], ref args[7], ref args[7], ref args[7]);
                        return null;
                    case 8:
                        ((IVoidArgs8AnonymousMethod)prop).InvokeAnonymous(instance, ref args[8], ref args[8], ref args[8], ref args[8], ref args[8], ref args[8], ref args[8], ref args[8]);
                        return null;
                    case 9:
                        ((IVoidArgs9AnonymousMethod)prop).InvokeAnonymous(instance, ref args[9], ref args[9], ref args[9], ref args[9], ref args[9], ref args[9], ref args[9], ref args[9], ref args[9]);
                        return null;
                    case 10:
                        ((IVoidArgs10AnonymousMethod)prop).InvokeAnonymous(instance, ref args[10], ref args[10], ref args[10], ref args[10], ref args[10], ref args[10], ref args[10], ref args[10], ref args[10], ref args[10]);
                        return null;
                    case 11:
                        ((IVoidArgs11AnonymousMethod)prop).InvokeAnonymous(instance, ref args[11], ref args[11], ref args[11], ref args[11], ref args[11], ref args[11], ref args[11], ref args[11], ref args[11], ref args[11], ref args[11]);
                        return null;
                    case 12:
                        ((IVoidArgs12AnonymousMethod)prop).InvokeAnonymous(instance, ref args[12], ref args[12], ref args[12], ref args[12], ref args[12], ref args[12], ref args[12], ref args[12], ref args[12], ref args[12], ref args[12], ref args[12]);
                        return null;
                    case 13:
                        ((IVoidArgs13AnonymousMethod)prop).InvokeAnonymous(instance, ref args[13], ref args[13], ref args[13], ref args[13], ref args[13], ref args[13], ref args[13], ref args[13], ref args[13], ref args[13], ref args[13], ref args[13], ref args[13]);
                        return null;
                    case 14:
                        ((IVoidArgs14AnonymousMethod)prop).InvokeAnonymous(instance, ref args[14], ref args[14], ref args[14], ref args[14], ref args[14], ref args[14], ref args[14], ref args[14], ref args[14], ref args[14], ref args[14], ref args[14], ref args[14], ref args[14]);
                        return null;
                    case 15:
                        ((IVoidArgs15AnonymousMethod)prop).InvokeAnonymous(instance, ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15]);
                        return null;
                    default:
                        ((IVoidArgsAnyAnonymousMethod)prop).InvokeAnonymous(instance,args);
                        return null;
                }
            }
            switch (prop.ArgumentTypes.Count)
            {
                case 0:
                    return ((IArgs0AnonymousMethod)prop).InvokeAnonymous(instance);
                case 1:
                    return ((IArgs1AnonymousMethod)prop).InvokeAnonymous(instance, ref args[1]);
                case 2:
                    return ((IArgs2AnonymousMethod)prop).InvokeAnonymous(instance, ref args[2], ref args[2]);
                case 3:
                    return ((IArgs3AnonymousMethod)prop).InvokeAnonymous(instance, ref args[3], ref args[3], ref args[3]);
                case 4:
                    return ((IArgs4AnonymousMethod)prop).InvokeAnonymous(instance, ref args[4], ref args[4], ref args[4], ref args[4]);
                case 5:
                    return ((IArgs5AnonymousMethod)prop).InvokeAnonymous(instance, ref args[5], ref args[5], ref args[5], ref args[5], ref args[5]);
                case 6:
                    return ((IArgs6AnonymousMethod)prop).InvokeAnonymous(instance, ref args[6], ref args[6], ref args[6], ref args[6], ref args[6], ref args[6]);
                case 7:
                    return ((IArgs7AnonymousMethod)prop).InvokeAnonymous(instance, ref args[7], ref args[7], ref args[7], ref args[7], ref args[7], ref args[7], ref args[7]);
                case 8:
                    return ((IArgs8AnonymousMethod)prop).InvokeAnonymous(instance, ref args[8], ref args[8], ref args[8], ref args[8], ref args[8], ref args[8], ref args[8], ref args[8]);
                case 9:
                    return ((IArgs9AnonymousMethod)prop).InvokeAnonymous(instance, ref args[9], ref args[9], ref args[9], ref args[9], ref args[9], ref args[9], ref args[9], ref args[9], ref args[9]);
                case 10:
                    return ((IArgs10AnonymousMethod)prop).InvokeAnonymous(instance, ref args[10], ref args[10], ref args[10], ref args[10], ref args[10], ref args[10], ref args[10], ref args[10], ref args[10], ref args[10]);
                case 11:
                    return ((IArgs11AnonymousMethod)prop).InvokeAnonymous(instance, ref args[11], ref args[11], ref args[11], ref args[11], ref args[11], ref args[11], ref args[11], ref args[11], ref args[11], ref args[11], ref args[11]);
                case 12:
                    return ((IArgs12AnonymousMethod)prop).InvokeAnonymous(instance, ref args[12], ref args[12], ref args[12], ref args[12], ref args[12], ref args[12], ref args[12], ref args[12], ref args[12], ref args[12], ref args[12], ref args[12]);
                case 13:
                    return ((IArgs13AnonymousMethod)prop).InvokeAnonymous(instance, ref args[13], ref args[13], ref args[13], ref args[13], ref args[13], ref args[13], ref args[13], ref args[13], ref args[13], ref args[13], ref args[13], ref args[13], ref args[13]);
                case 14:
                    return ((IArgs14AnonymousMethod)prop).InvokeAnonymous(instance, ref args[14], ref args[14], ref args[14], ref args[14], ref args[14], ref args[14], ref args[14], ref args[14], ref args[14], ref args[14], ref args[14], ref args[14], ref args[14], ref args[14]);
                case 15:
                    return ((IArgs15AnonymousMethod)prop).InvokeAnonymous(instance, ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15], ref args[15]);
                default:
                    return ((IArgsAnyAnonymousMethod)prop).InvokeAnonymous(instance, args);
            }
        }
        public static IArgsAnyAnonymousMethod GetArgsAnyAnonymousMethod(this ITypeDefine typeDefine, string name)
        {
            var prop = typeDefine.Methods.FirstOrDefault(x => x.Name == name);
            if (prop == null)
            {
                throw new InvalidOperationException($"Type {typeDefine.Name} has not method {name}");
            }
            if (prop is not IArgsAnyAnonymousMethod argsAnyAnonymousMethod)
            {
                throw new InvalidOperationException($"Type {typeDefine.Name}.{name} must implement {typeof(IArgsAnyAnonymousMethod)}");
            }
            return argsAnyAnonymousMethod;
        }
        public static IVoidArgsAnyAnonymousMethod GetVoidArgsAnyAnonymousMethod(this ITypeDefine typeDefine, string name)
        {
            var prop = typeDefine.Methods.FirstOrDefault(x => x.Name == name);
            if (prop == null)
            {
                throw new InvalidOperationException($"Type {typeDefine.Name} has not method {name}");
            }
            if (prop is not IVoidArgsAnyAnonymousMethod voidArgsAnyAnonymousMethod)
            {
                throw new InvalidOperationException($"Type {typeDefine.Name}.{name} must implement {typeof(IVoidArgsAnyAnonymousMethod)}");
            }
            return voidArgsAnyAnonymousMethod;
        }
        public static object GetProperty(this ITypeDefine typeDefine, object? instance, string name)
        {
            return GetPropertyAnonymousInvoke(typeDefine, name).GetValueAnonymous(instance);
        }
    }
}
