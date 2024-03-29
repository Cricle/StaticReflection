﻿using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Text;

namespace StaticReflection.CodeGen.Generators
{
    partial class Parser
    {
        private void GetMethodDefine(INamedTypeSymbol targetType,
            IMethodSymbol method,
            out string implementInvokeInterface,
            out string invokeImplement,
            out string invokeNoRefImplement,
            out bool argOutof16,
            out bool hasReturn,
            out string returnType,
            out string unsafeScript)
        {
            argOutof16 = method.Parameters.Length > 16;
            hasReturn = !method.ReturnsVoid || method.MethodKind == MethodKind.Constructor;
            implementInvokeInterface = ",";
            returnType = method.MethodKind == MethodKind.Constructor ? method.ContainingType.ToString() : method.ReturnType.ToString();
            unsafeScript = @"
#if !NET7_0_OR_GREATER
unsafe
#endif
";
            invokeNoRefImplement = "[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]\n";
            invokeImplement = "[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]\n";
            if (argOutof16)
            {
                if (hasReturn)
                {
                    implementInvokeInterface += $"StaticReflection.Invoking.IArgsAnyMethod<{targetType},{returnType}>,StaticReflection.Invoking.IArgsAnyAnonymousMethod";
                    invokeImplement += $"public {unsafeScript} ref {returnType} Invoke({targetType} instance, params object[] inputs)";
                }
                else
                {
                    implementInvokeInterface += $"StaticReflection.Invoking.IVoidArgsAnyMethod<{targetType}>,StaticReflection.Invoking.IVoidArgsAnyAnonymousMethod";
                    invokeImplement += $"public {unsafeScript} void Invoke({targetType} instance, params object[] inputs)";
                }
            }
            else if (method.Parameters.Length == 0)
            {
                if (hasReturn)
                {
                    implementInvokeInterface += $"StaticReflection.Invoking.IArgsMethod<{targetType},{returnType}>,StaticReflection.Invoking.IArgs{method.Parameters.Length}AnonymousMethod,StaticReflection.Invoking.IUsualArgsMethod<{targetType},{returnType}>,StaticReflection.Invoking.IUsualArgs{method.Parameters.Length}AnonymousMethod";
                    invokeImplement += $"public {unsafeScript} ref {returnType} Invoke({targetType} instance)";
                    invokeNoRefImplement += $"public {returnType} InvokeUsual({targetType} instance)";
                }
                else
                {
                    implementInvokeInterface += $"StaticReflection.Invoking.IVoidArgsMethod<{targetType}>,StaticReflection.Invoking.IVoidArgs{method.Parameters.Length}AnonymousMethod,StaticReflection.Invoking.IUsualVoidArgsMethod<{targetType}>,StaticReflection.Invoking.IUsualVoidArgs{method.Parameters.Length}AnonymousMethod";
                    invokeImplement += $"public {unsafeScript} void Invoke({targetType} instance)";
                    invokeNoRefImplement += $"public void InvokeUsual({targetType} instance)";
                }
            }
            else
            {
                var args = string.Join(",", method.Parameters.Select((x, i) => $"ref {x.Type} arg{i}"));
                var argsNoRef = string.Join(",", method.Parameters.Select((x, i) => $"{x.Type} arg{i}"));
                var usualImpl = string.Empty;
                if (hasReturn)
                {
                    implementInvokeInterface += $"StaticReflection.Invoking.IArgsMethod<{targetType},{returnType},";
                    usualImpl += $"StaticReflection.Invoking.IUsualArgsMethod<{targetType},{returnType},";
                    invokeImplement += $"public {unsafeScript} ref {returnType} Invoke({targetType} instance,{args})";
                    invokeNoRefImplement += $"public {returnType} InvokeUsual({targetType} instance,{argsNoRef})";
                }
                else
                {
                    implementInvokeInterface += $"StaticReflection.Invoking.IVoidArgsMethod<{targetType},";
                    usualImpl += $"StaticReflection.Invoking.IUsualVoidArgsMethod<{targetType},";
                    invokeImplement += $"public {unsafeScript} void Invoke({targetType} instance,{args})";
                    invokeNoRefImplement += $"public void InvokeUsual({targetType} instance,{argsNoRef})";
                }
                usualImpl += string.Join(",", method.Parameters.Select(x => x.Type)) + ">";
                implementInvokeInterface += string.Join(",", method.Parameters.Select(x => x.Type)) + ">";
                if (hasReturn)
                {
                    implementInvokeInterface += $",StaticReflection.Invoking.IArgs{method.Parameters.Length}AnonymousMethod";
                    usualImpl += $",StaticReflection.Invoking.IUsualArgs{method.Parameters.Length}AnonymousMethod";
                }
                else
                {
                    implementInvokeInterface += $",StaticReflection.Invoking.IVoidArgs{method.Parameters.Length}AnonymousMethod";
                    usualImpl += $",StaticReflection.Invoking.IUsualVoidArgs{method.Parameters.Length}AnonymousMethod";
                }
                if (!string.IsNullOrEmpty(usualImpl))
                {
                    implementInvokeInterface += "," + usualImpl;
                }
            }
        }

        private string CreateMethodProperies(string targetType, IMethodSymbol method, SemanticModel model)
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

        public StaticReflection.StaticMethodKind MethodKind {{ get; }} = StaticReflection.StaticMethodKind.{method.MethodKind};  

        public StaticReflection.StaticRefKind RefKind {{ get; }} = StaticReflection.StaticRefKind.{method.RefKind};        

        public StaticReflection.StaticNullableAnnotation ReturnNullableAnnotation {{ get; }} = StaticReflection.StaticNullableAnnotation.{method.ReturnNullableAnnotation};        

        public StaticReflection.StaticNullableAnnotation ReceiverNullableAnnotation {{ get; }} = StaticReflection.StaticNullableAnnotation.{method.ReceiverNullableAnnotation};        
 
        public System.Boolean ReturnsByRefReadonly {{ get; }} = {BoolToString(method.ReturnsByRefReadonly)};        

        public System.Type ReturnType {{ get; }} = typeof({(method.MethodKind == MethodKind.Constructor ? method.ContainingType : method.ReturnType)});     

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

        public System.Collections.Generic.IReadOnlyList<System.Attribute> ReturnTypeAttributes {{ get; }} = new System.Attribute[] {{ {string.Join(",", GetAttributeStrings(model, method.GetReturnTypeAttributes()))} }};

";
        }
        private string BuildPropertyClass(string name, INamedTypeSymbol targetType, IMethodSymbol method, GeneratorTransformResult<ISymbol> result)
        {
            var model = result.SyntaxContext.SemanticModel;
            var implementInvokeInterface = string.Empty;
            var invokeImplement = string.Empty;
            var invokeNoRefImplement = string.Empty;
            
            if ((!method.IsGenericMethod && result.IsAvaliableVisibility(method) || 
                (method.MethodKind == MethodKind.Constructor && !method.IsStatic && result.IsAvaliableVisibility(method)))&&
                (!method.ReturnsByRef&&method.Parameters.All(x=>(x.RefKind== RefKind.None||x.RefKind== RefKind.In)&&!x.Type.IsRefLikeType)))
            {
                GetMethodDefine(targetType,
                    method,
                    out implementInvokeInterface,
                    out invokeImplement,
                    out invokeNoRefImplement,
                    out bool argOutof16,
                    out bool hasReturn,
                    out string returnType,
                    out string unsafeScript);
                invokeImplement += "\n{\n";
                invokeNoRefImplement += "\n{\n";

                var initCodes = new StringBuilder();
                var bodyCodes = new List<string>();
                var bodyNoRefCodes = new List<string>();

                for (int i = 0; i < method.Parameters.Length; i++)
                {
                    var par = method.Parameters[i];
                    var line = CompileArg(par.Type.ToString(), "arg" + i, par, initCodes);
                    bodyCodes.Add(line);
                    bodyNoRefCodes.Add($"({par.Type})arg{i}");
                }
                var bodyCode = string.Join(",", bodyCodes);
                var bodyCodeNoRef = string.Join(",", bodyNoRefCodes);
                var call = string.Empty;
                var callNoRef = string.Empty;
                if (method.MethodKind == MethodKind.Constructor)
                {
                    call = $"new {method.ContainingType.Name}({bodyCode})";
                    callNoRef = $"new {method.ContainingType.Name}({bodyCodeNoRef})";
                }
                else
                {
                    call = $"instance.{method.Name}({bodyCode})";
                    callNoRef = $"instance.{method.Name}({bodyCodeNoRef})";
                    if (method.IsStatic)
                    {
                        call = $"{targetType.Name}.{method.Name}({bodyCode})";
                        callNoRef = $"{targetType.Name}.{method.Name}({bodyCodeNoRef})";
                    }
                }
                if (hasReturn)
                {
                    callNoRef = " return " + callNoRef + ";";
                    call = $@"
ref {returnType} result = ref System.Runtime.CompilerServices.Unsafe.AsRef({call});
return ref result;
";
                }
                else
                {
                    call += ";";
                    callNoRef += ";";
                }
                invokeImplement += initCodes;
                invokeImplement += $"\n{call}\n}}\n";
                invokeNoRefImplement += $"\n{callNoRef}\n}}\n";

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
                    var argNoRefStr = string.Join(",", Enumerable.Range(0, method.Parameters.Length).Select(x => $"object arg{x}"));
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
                    var parNoRefStr = string.Join(",", Enumerable.Range(0, method.Parameters.Length)
                        .Select(x => $"({method.Parameters[x].Type})arg{x}"));
                    if (method.Parameters.Length != 0)
                    {
                        argStr = "," + argStr;
                        argNoRefStr = "," + argNoRefStr;
                        parStr = "," + parStr;
                        parNoRefStr = "," + parNoRefStr;
                    }
                    if (hasReturn)
                    {
                        invokeImplement += $@"
public {unsafeScript} ref object InvokeAnonymous(object instance{argStr})
{{
    return ref System.Runtime.CompilerServices.Unsafe.AsRef<object>(Invoke({asInstance}{parStr}));
}}
";
                        invokeNoRefImplement += $@"
public object InvokeUsualAnonymous(object instance{argNoRefStr})
{{
    return InvokeUsual({asInstance}{parNoRefStr});
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
                        invokeNoRefImplement += $@"
public void InvokeUsualAnonymous(object instance{argNoRefStr})
{{
    InvokeUsual({asInstance}{parNoRefStr});
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
                else if (symbol.RefKind == RefKind.RefReadOnly)
                {
                    return $"ref System.Runtime.CompilerServices.Unsafe.{method}{inputArg})";
                }
                else
                {
                    throw new NotSupportedException(symbol.RefKind.ToString());
                }
            }

            var selectDefine = method.MethodKind == MethodKind.Constructor ? "StaticReflection.IConstructorDefine" : "StaticReflection.IMethodDefine";
            var str = $@"
    {GenHeaders.AttackAttribute}
    {visibility} sealed class {name} : {selectDefine}{implementInvokeInterface}
    {{
        public static readonly {name} Instance = new {name}();

        private {name}(){{ }}

        {CreateSymbolProperties(model, method)}
        {CreateMethodProperies(targetType.ToString(), method, model)}
        {invokeImplement}
        {invokeNoRefImplement}
    }}
";
            return str;
        }

        protected List<string> ExecuteMethods(SourceProductionContext context, GeneratorTransformResult<ISymbol> node, INamedTypeSymbol targetType)
        {
            var members = targetType.GetMembers();
            var methods = members.OfType<IMethodSymbol>()
                .Where(x => (x.MethodKind == MethodKind.Ordinary) && !x.IsGenericMethod && (!x.Name.StartsWith("<") || !x.Name.EndsWith("$")))
                .Where(x => !x.Parameters.Any(x => x.Type.IsRefLikeType) && !x.ReturnsByRef)
                .ToList();
            if (methods.Count == 0)
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
                var ssr = name + index + "MReflection";
                index++;

                var str = BuildPropertyClass(ssr, targetType, method, node);
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
