using StaticReflection.Annotions;
using StaticReflection.Invoking;
using System.Reflection;

namespace StaticReflection
{
    public static class TypeDefineExtensions
    {
        private static readonly Type staticReflectionType = typeof(StaticReflectionAssemblyAttribute);
        private static readonly object findAssemblyLocker = new object();
        private static readonly Dictionary<Assembly, IAssemblyDefine?> assemblyDefineMapper = new Dictionary<Assembly, IAssemblyDefine?>();

        public static IAssemblyDefine? FindStaticAssembly(this Type type)
        {
            var assembly = type.Assembly;
            if (!assemblyDefineMapper.TryGetValue(assembly, out var def))
            {
                lock (findAssemblyLocker)
                {
                    if (!assemblyDefineMapper.TryGetValue(assembly, out def))
                    {
                        def = CoreFindStaticAssembly(assembly);
                        assemblyDefineMapper.Add(assembly, def);
                    }
                }
            }
            return def;
        }
        private static IAssemblyDefine? CoreFindStaticAssembly(Assembly assembly)
        {
            var assemblyType = assembly.GetTypes().Where(x => x.IsDefined(staticReflectionType)).FirstOrDefault();
            if (assemblyType != null)
            {
                var prop = assemblyType.GetProperty("Default", BindingFlags.Static | BindingFlags.Public);
                if (prop != null)
                {
                    return (IAssemblyDefine)prop.GetValue(null);
                }
                var constructor = assemblyType.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    return (IAssemblyDefine)constructor.Invoke(null);
                }
            }
            return null;
        }

        public static ITypeDefine? FindType(this IAssemblyDefine assembly, string typeName)
        {
            return assembly.Types.FirstOrDefault(x => x.Name == typeName);
        }
        public static IEventDefine? FindEvent(this ITypeDefine type, string eventName)
        {
            return type.Events.FirstOrDefault(x => x.Name == eventName);
        }
        public static IEventTransfer? FindEventTransfer(this ITypeDefine type, string eventName)
        {
            return (IEventTransfer?)type.Events.FirstOrDefault(x => x is IEventTransfer && x.Name == eventName);
        }
        public static IMethodDefine? FindMethod(this ITypeDefine type, string methodName)
        {
            return (IMethodDefine?)type.Methods.FirstOrDefault(x => x.Name == methodName);
        }
        public static IConstructorDefine? FindEmptyConstructor(this ITypeDefine type)
        {
            return (IConstructorDefine?)type.Constructors.FirstOrDefault(x => x.ArgumentTypes.Count == 0);
        }
        public static void SetProperty(this ITypeDefine typeDefine, object? instance, string name, object? value)
        {
            GetPropertyAnonymousInvoke(typeDefine, name).SetValueAnonymous(instance, value);
        }
        public static IMemberAnonymousInvokeDefine GetPropertyAnonymousInvoke(this ITypeDefine typeDefine, string name)
        {
            var prop = typeDefine.Properties.FirstOrDefault(x => x.Name == name);
            if (prop == null)
            {
                throw new InvalidOperationException($"Type {typeDefine.Name} has not property {name}");
            }
            if (prop is not IMemberAnonymousInvokeDefine anonymousInvokeDefine)
            {
                throw new InvalidOperationException($"Type {typeDefine.Name}.{name} must implement {typeof(IMemberAnonymousInvokeDefine)}");
            }
            return anonymousInvokeDefine;
        }
        public static object? InvokeUsualMethod(this IMethodDefine method, object? instance, params object?[] args)
        {
            if (method.ReturnType == typeof(void))
            {
                switch (method.ArgumentTypes.Count)
                {
                    case 0:
                        ((IUsualVoidArgs0AnonymousMethod)method).InvokeUsualAnonymous(instance);
                        return null;
                    case 1:
                        ((IUsualVoidArgs1AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0]);
                        return null;
                    case 2:
                        ((IUsualVoidArgs2AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1]);
                        return null;
                    case 3:
                        ((IUsualVoidArgs3AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2]);
                        return null;
                    case 4:
                        ((IUsualVoidArgs4AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3]);
                        return null;
                    case 5:
                        ((IUsualVoidArgs5AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4]);
                        return null;
                    case 6:
                        ((IUsualVoidArgs6AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5]);
                        return null;
                    case 7:
                        ((IUsualVoidArgs7AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                        return null;
                    case 8:
                        ((IUsualVoidArgs8AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                        return null;
                    case 9:
                        ((IUsualVoidArgs9AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                        return null;
                    case 10:
                        ((IUsualVoidArgs10AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
                        return null;
                    case 11:
                        ((IUsualVoidArgs11AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]);
                        return null;
                    case 12:
                        ((IUsualVoidArgs12AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]);
                        return null;
                    case 13:
                        ((IUsualVoidArgs13AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12]);
                        return null;
                    case 14:
                        ((IUsualVoidArgs14AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13]);
                        return null;
                    case 15:
                        ((IUsualVoidArgs15AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14]);
                        return null;
                    default:
                        ((IVoidArgsAnyAnonymousMethod)method).InvokeAnonymous(instance, args);
                        return null;
                }
            }
            switch (method.ArgumentTypes.Count)
            {
                case 0:
                    return ((IUsualArgs0AnonymousMethod)method).InvokeUsualAnonymous(instance);
                case 1:
                    return ((IUsualArgs1AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0]);
                case 2:
                    return ((IUsualArgs2AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1]);
                case 3:
                    return ((IUsualArgs3AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2]);
                case 4:
                    return ((IUsualArgs4AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3]);
                case 5:
                    return ((IUsualArgs5AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4]);
                case 6:
                    return ((IUsualArgs6AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5]);
                case 7:
                    return ((IUsualArgs7AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                case 8:
                    return ((IUsualArgs8AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                case 9:
                    return ((IUsualArgs9AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                case 10:
                    return ((IUsualArgs10AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
                case 11:
                    return ((IUsualArgs11AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]);
                case 12:
                    return ((IUsualArgs12AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]);
                case 13:
                    return ((IUsualArgs13AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12]);
                case 14:
                    return ((IUsualArgs14AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13]);
                case 15:
                    return ((IUsualArgs15AnonymousMethod)method).InvokeUsualAnonymous(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14]);
                default:
                    return ((IArgsAnyAnonymousMethod)method).InvokeAnonymous(instance, args);
            }

        }

        public static object? InvokeMethod(this IMethodDefine method, object? instance, params object?[] args)
        {
            if (method.ReturnType == typeof(void))
            {
                switch (method.ArgumentTypes.Count)
                {
                    case 0:
                        ((IVoidArgs0AnonymousMethod)method).InvokeAnonymous(instance);
                        return null;
                    case 1:
                        ((IVoidArgs1AnonymousMethod)method).InvokeAnonymous(instance, ref args[0]);
                        return null;
                    case 2:
                        ((IVoidArgs2AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1]);
                        return null;
                    case 3:
                        ((IVoidArgs3AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2]);
                        return null;
                    case 4:
                        ((IVoidArgs4AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3]);
                        return null;
                    case 5:
                        ((IVoidArgs5AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4]);
                        return null;
                    case 6:
                        ((IVoidArgs6AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5]);
                        return null;
                    case 7:
                        ((IVoidArgs7AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5], ref args[6]);
                        return null;
                    case 8:
                        ((IVoidArgs8AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5], ref args[6], ref args[7]);
                        return null;
                    case 9:
                        ((IVoidArgs9AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5], ref args[6], ref args[7], ref args[8]);
                        return null;
                    case 10:
                        ((IVoidArgs10AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5], ref args[6], ref args[7], ref args[8], ref args[9]);
                        return null;
                    case 11:
                        ((IVoidArgs11AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5], ref args[6], ref args[7], ref args[8], ref args[9], ref args[10]);
                        return null;
                    case 12:
                        ((IVoidArgs12AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5], ref args[6], ref args[7], ref args[8], ref args[9], ref args[10], ref args[11]);
                        return null;
                    case 13:
                        ((IVoidArgs13AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5], ref args[6], ref args[7], ref args[8], ref args[9], ref args[10], ref args[11], ref args[12]);
                        return null;
                    case 14:
                        ((IVoidArgs14AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5], ref args[6], ref args[7], ref args[8], ref args[9], ref args[10], ref args[11], ref args[12], ref args[13]);
                        return null;
                    case 15:
                        ((IVoidArgs15AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5], ref args[6], ref args[7], ref args[8], ref args[9], ref args[10], ref args[11], ref args[12], ref args[13], ref args[14]);
                        return null;
                    default:
                        ((IVoidArgsAnyAnonymousMethod)method).InvokeAnonymous(instance, args);
                        return null;
                }
            }
            switch (method.ArgumentTypes.Count)
            {
                case 0:
                    return ((IArgs0AnonymousMethod)method).InvokeAnonymous(instance);
                case 1:
                    return ((IArgs1AnonymousMethod)method).InvokeAnonymous(instance, ref args[0]);
                case 2:
                    return ((IArgs2AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1]);
                case 3:
                    return ((IArgs3AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2]);
                case 4:
                    return ((IArgs4AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3]);
                case 5:
                    return ((IArgs5AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4]);
                case 6:
                    return ((IArgs6AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5]);
                case 7:
                    return ((IArgs7AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5], ref args[6]);
                case 8:
                    return ((IArgs8AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5], ref args[6], ref args[7]);
                case 9:
                    return ((IArgs9AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5], ref args[6], ref args[7], ref args[8]);
                case 10:
                    return ((IArgs10AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5], ref args[6], ref args[7], ref args[8], ref args[9]);
                case 11:
                    return ((IArgs11AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5], ref args[6], ref args[7], ref args[8], ref args[9], ref args[10]);
                case 12:
                    return ((IArgs12AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5], ref args[6], ref args[7], ref args[8], ref args[9], ref args[10], ref args[11]);
                case 13:
                    return ((IArgs13AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5], ref args[6], ref args[7], ref args[8], ref args[9], ref args[10], ref args[11], ref args[12]);
                case 14:
                    return ((IArgs14AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5], ref args[6], ref args[7], ref args[8], ref args[9], ref args[10], ref args[11], ref args[12], ref args[13]);
                case 15:
                    return ((IArgs15AnonymousMethod)method).InvokeAnonymous(instance, ref args[0], ref args[1], ref args[2], ref args[3], ref args[4], ref args[5], ref args[6], ref args[7], ref args[8], ref args[9], ref args[10], ref args[11], ref args[12], ref args[13], ref args[14]);
                default:
                    return ((IArgsAnyAnonymousMethod)method).InvokeAnonymous(instance, args);
            }

        }
        public static object? InvokeMethod(this ITypeDefine typeDefine, string name, object? instance, params object?[] args)
        {
            var method = typeDefine.Methods.FirstOrDefault(x => x.Name == name && x.ArgumentTypes.Count == args.Length);
            if (method == null)
            {
                throw new InvalidOperationException($"Type {typeDefine.Name} has not method {name}");
            }
            return InvokeMethod(method, instance, args);
        }
        public static object? InvokeUsualMethod(this ITypeDefine typeDefine, string name, object? instance, params object?[] args)
        {
            var method = typeDefine.Methods.FirstOrDefault(x => x.Name == name && x.ArgumentTypes.Count == args.Length);
            if (method == null)
            {
                throw new InvalidOperationException($"Type {typeDefine.Name} has not method {name}");
            }
            return InvokeUsualMethod(method, instance, args);
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
