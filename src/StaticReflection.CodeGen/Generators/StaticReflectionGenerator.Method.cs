using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;

namespace StaticReflection.CodeGen.Generators
{
    public partial class StaticReflectionGenerator
    {
        private string CreateMethodProperies(string targetType,IMethodSymbol method)
        {
            var typePars = new List<string>();

            foreach (var item in method.TypeParameters)
            {
                var typePar = $"new StaticReflection.TypeArgumentDefine(\"{item.Name}\",typeof({targetType}),{BoolToString(item.HasReferenceTypeConstraint)},{BoolToString(item.HasValueTypeConstraint)},{BoolToString(item.HasUnmanagedTypeConstraint)},{BoolToString(item.HasNotNullConstraint)},{BoolToString(item.HasConstructorConstraint)},new System.Type[] {{ {string.Join(",", item.ConstraintTypes.Select(x => $"typeof({x})"))} }})";
                typePars.Add(typePar);
            }
            return $@"

        public System.Type DeclareType {{ get; }} = typeof({targetType});
        
        public System.Boolean ReturnsByRef {{ get; }} = {BoolToString(method.ReturnsByRef)};        

        public StaticReflection.StaticMethodKind MethodKind {{ get; }} = StaticMethodKind.{method.MethodKind};        
 
        public System.Boolean ReturnsByRefReadonly {{ get; }} = {BoolToString(method.ReturnsByRefReadonly)};        

        public System.Type ReturnType {{ get; }} = typeof({method.ReturnType});     

        public System.Collections.Generic.IReadOnlyList<System.Type> ArgumentTypes {{ get; }} = new System.Type[]{{ {string.Join(",", method.Parameters.Select(x => $"typeof({x.Type.ToString().TrimEnd('?')})"))} }};        
         
        public System.Boolean IsGenericMethod {{ get; }} = {BoolToString(method.IsGenericMethod)};      

        public System.Int32 Arity {{ get; }} = {method.Arity};        

        public System.Boolean IsExtensionMethod {{ get; }} = {BoolToString(method.IsExtensionMethod)}; 

        public System.Boolean IsAsync {{ get; }} = {BoolToString(method.IsAsync)};  

        public System.Boolean IsVararg {{ get; }} = {BoolToString(method.IsVararg)};  

        public System.Boolean IsCheckedBuiltin {{ get; }} = {BoolToString(method.IsCheckedBuiltin)};     

        public System.Boolean HidesBaseMethodsByName {{ get; }} = {BoolToString(method.HidesBaseMethodsByName)};     

        public System.Boolean ReturnsVoid {{ get; }} = {BoolToString(method.ReturnsVoid)};    

        public System.Boolean IsReadOnly {{ get; }} = {BoolToString(method.IsReadOnly)};     

        public System.Boolean IsInitOnly {{ get; }} = {BoolToString(method.IsInitOnly)};  

        public System.Boolean IsPartialDefinition {{ get; }} = {BoolToString(method.IsPartialDefinition)}; 

        public System.Boolean IsConditional {{ get; }} = {BoolToString(method.IsConditional)};     
        
        public System.Collections.Generic.IReadOnlyList<StaticReflection.ITypeArgumentDefine> TypeArguments {{ get; }} = new StaticReflection.ITypeArgumentDefine[] {{ {string.Join(",", typePars)} }};      
";
        }
        private string BuildPropertyClass(string name,INamedTypeSymbol targetType, IMethodSymbol method)
        {
            var implementInvokeInterface = string.Empty;
            var invokeImplement = string.Empty;

            if (!method.IsGenericMethod&&IsAvaliableVisibility(method))
            {
                var argOutof16 = method.Parameters.Length > 16;
                var hasReturn = !method.ReturnsVoid;
                implementInvokeInterface = ",";
                var unsafeScript = @"
#if !NET7_0_OR_GREATER
unsafe
#endif
";
                if (argOutof16)
                {
                    if (hasReturn)
                    {
                        implementInvokeInterface += $"StaticReflection.Invoking.IArgsAnyMethod<{targetType},{method.ReturnType}>,StaticReflection.Invoking.IArgsAnyAnonymousMethod";
                        invokeImplement = $"public {unsafeScript} ref {method.ReturnType} Invoke({targetType} instance, params object[] inputs)";
                    }
                    else
                    {
                        implementInvokeInterface += $"StaticReflection.Invoking.IVoidArgsAnyMethod<{targetType}>,StaticReflection.Invoking.IVoidArgsAnyAnonymousMethod";
                        invokeImplement = $"public {unsafeScript} void Invoke({targetType} instance, params object[] inputs)";
                    }
                }
                else if (method.Parameters.Length == 0)
                {
                    if (hasReturn)
                    {
                        implementInvokeInterface += $"StaticReflection.Invoking.IArgsMethod<{targetType},{method.ReturnType}>,StaticReflection.Invoking.IArgs{method.Parameters.Length}AnonymousMethod";
                        invokeImplement = $"public {unsafeScript} ref {method.ReturnType} Invoke({targetType} instance)";
                    }
                    else
                    {
                        implementInvokeInterface += $"StaticReflection.Invoking.IVoidArgsMethod<{targetType}>,StaticReflection.Invoking.IVoidArgs{method.Parameters.Length}AnonymousMethod";
                        invokeImplement = $"public {unsafeScript} void Invoke({targetType} instance)";
                    }
                }
                else
                {
                    var args = string.Join(",", method.Parameters.Select((x, i) => $"ref {x.Type} arg{i}"));
                    if (hasReturn)
                    {
                        implementInvokeInterface += $"StaticReflection.Invoking.IArgsMethod<{targetType},{method.ReturnType},";
                        invokeImplement = $"public {unsafeScript} ref {method.ReturnType} Invoke({targetType} instance,{args})";
                    }
                    else
                    {
                        implementInvokeInterface += $"StaticReflection.Invoking.IVoidArgsMethod<{targetType},";
                        invokeImplement = $"public {unsafeScript} void Invoke({targetType} instance,{args})";
                    }
                    implementInvokeInterface += string.Join(",", method.Parameters.Select(x => x.Type)) + ">";
                    if (hasReturn)
                    {
                        implementInvokeInterface += $",StaticReflection.Invoking.IArgs{method.Parameters.Length}AnonymousMethod";
                    }
                    else
                    {
                        implementInvokeInterface += $",StaticReflection.Invoking.IVoidArgs{method.Parameters.Length}AnonymousMethod";
                    }
                }
                invokeImplement += "\n{\n";

                var initCodes = new StringBuilder();
                var bodyCodes = new List<string>();

                for (int i = 0; i < method.Parameters.Length; i++)
                {
                    var par = method.Parameters[i];
                    var line = CompileArg(par.Type.ToString(), "arg" + i, par, initCodes);
                    bodyCodes.Add(line);
                }
                var bodyCode = string.Join(",", bodyCodes);
                var call = $"instance.{method.Name}({bodyCode})";
                if (method.IsStatic)
                {
                    call = $"{targetType.Name}.{method.Name}({bodyCode})";
                }
                if (hasReturn)
                {
                    call = $@"
ref {method.ReturnType} result = ref System.Runtime.CompilerServices.Unsafe.AsRef({call});
return ref result;
";
                }
                else
                {
                    call += ";";
                }
                invokeImplement += initCodes;
                invokeImplement += $"\n{call}\n}}\n";

                var asInstance = $"({targetType})instance";
                if (argOutof16)
                {
                    if (hasReturn)
                    {
                        invokeImplement += $@"
public {unsafeScript} ref object InvokeAnonymous(object instance, params object[] inputs)
{{
    return ref System.Runtime.CompilerServices.Unsafe.AsRef<object>(Invoke({asInstance},inputs));
}}
";
                    }
                    else
                    {
                        invokeImplement += $@"
public void InvokeAnonymous(object instance, params object[] inputs)
{{
    Invoke({asInstance},inputs);
}}
";
                    }
                }
                else
                {
                    var argStr = string.Join(",", Enumerable.Range(0, method.Parameters.Length).Select(x => $"ref object arg{x}"));
                    var parStr = string.Join(",", Enumerable.Range(0, method.Parameters.Length)
                        .Select(x =>
                        {
                            var par = method.Parameters[x];
                            if (par.Type.IsValueType)
                            {
                                return $"ref System.Runtime.CompilerServices.Unsafe.Unbox<{par.Type}>(arg{x})";
                            }
                            return $"ref System.Runtime.CompilerServices.Unsafe.As<object,{par.Type}>(ref arg{x})";
                        }));
                    if (method.Parameters.Length != 0)
                    {
                        argStr = "," + argStr;
                        parStr = "," + parStr;
                    }
                    if (hasReturn)
                    {
                        invokeImplement += $@"
public {unsafeScript} ref object InvokeAnonymous(object instance{argStr})
{{
    return ref System.Runtime.CompilerServices.Unsafe.AsRef<object>(Invoke({asInstance}{parStr}));
}}
";
                    }
                    else
                    {
                        invokeImplement += $@"
public void InvokeAnonymous(object instance{argStr})
{{
    Invoke({asInstance}{parStr});
}}
";
                    }
                }
            }
            var visibility = GetAccessibilityString(targetType.DeclaredAccessibility);

            string CompileArg(string inputType, string inputArg, IParameterSymbol symbol, StringBuilder initCodes)
            {
                var method = $"As<{inputType},{symbol.Type}>(ref ";
                if (symbol.Type.IsValueType)
                {
                    method = $"Unbox<{symbol.Type}>(";
                }

                if (symbol.RefKind == RefKind.None)
                {
                    return $"System.Runtime.CompilerServices.Unsafe.{method}{inputArg})";
                }
                else if (symbol.RefKind == RefKind.Ref)
                {
                    return $"ref System.Runtime.CompilerServices.Unsafe.{method}{inputArg})";
                }
                else if (symbol.RefKind == RefKind.RefReadOnly)
                {
                    return $"ref System.Runtime.CompilerServices.Unsafe.{method}{inputArg})";
                }
                else if (symbol.RefKind == RefKind.In)
                {
                    return $"in System.Runtime.CompilerServices.Unsafe.{method}{inputArg})";
                }
                else if (symbol.RefKind == RefKind.Out)
                {
                    var argName = "out" + inputArg;
                    initCodes.AppendLine($"ref {symbol.Type} {argName} = ref System.Runtime.CompilerServices.Unsafe.As<{inputType},{symbol.Type}>(ref {inputArg})");
                    return $"out {argName}";
                }
                else
                {
                    throw new NotSupportedException(symbol.RefKind.ToString());
                }
            }
            var str = $@"
    [System.Diagnostics.DebuggerStepThrough]
    [System.Runtime.CompilerServices.CompilerGenerated]
    {visibility} sealed class {name} : StaticReflection.IMethodDefine{implementInvokeInterface}
    {{
        public static readonly {name} Instance = new {name}();

        private {name}(){{ }}

        {CreateSymbolProperties(method)}
        {CreateMethodProperies(targetType.ToString(),method)}
        {invokeImplement}
    }}
";
            return str;
        }
        protected List<string> ExecuteMethods(SourceProductionContext context, GeneratorTransformResult<ISymbol> node, INamedTypeSymbol targetType)
        {
            var members = targetType.GetMembers();
            var methods = members.OfType<IMethodSymbol>()
                .Where(x => (x.MethodKind == MethodKind.Ordinary) &&!x.IsGenericMethod && (!x.Name.StartsWith("<") || !x.Name.EndsWith("$")))
                .ToList();
            if (methods.Count==0)
            {
                return new List<string>(0);
            }
            var visibility = GetAccessibilityString(targetType.DeclaredAccessibility);

            var name = targetType.ToString().Split('.').Last();

            var scriptBuilder = new StringBuilder();

            var types = new List<string>();
            var index = 0;
            foreach (var method in methods)
            {
                var ssr = name + method.Name + "T" + method.TypeParameters.Length + "P" + index + "MReflection";
                index++;

                var str = BuildPropertyClass(ssr, targetType, method);
                types.Add(ssr);

                scriptBuilder.AppendLine(str);
            }

            scriptBuilder.AppendLine();

            var code = FormatCode(scriptBuilder.ToString());
            sourceScript.AppendLine(code);
            return types;
        }
    }
}
