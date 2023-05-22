using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Text;
using System.Diagnostics;

namespace StaticReflection.CodeGen.Generators
{
    public partial class PropertyGenerator
    {
        protected void ExecuteMethods(SourceProductionContext context, GeneratorTransformResult<TypeDeclarationSyntax> node, INamedTypeSymbol targetType)
        {
            var members = targetType.GetMembers();
            var methods = members.OfType<IMethodSymbol>().Where(x=>x.MethodKind== MethodKind.Ordinary).ToList();
            var visibility = GetAccessibilityString(targetType.DeclaredAccessibility);

            var nameSpace = targetType.ContainingNamespace.ToString();
            var name = targetType.ToString().Split('.').Last();

            var scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine("#pragma warning disable CS9082");
            scriptBuilder.AppendLine($"namespace {nameSpace}");
            scriptBuilder.AppendLine("{");

            foreach (var method in methods)
            {
                var ssr = name + method.Name + "T" + method.TypeParameters.Length + "P" + method.Parameters.Length + "MReflection";
                var attributeStrs = GetAttributeStrings(method.GetAttributes());

                var typePars = new List<string>();

                foreach (var item in method.TypeParameters)
                {
                    var typePar = $"new StaticReflection.TypeArgumentDefine(\"{item.Name}\",typeof({targetType}),{BoolToString(item.HasReferenceTypeConstraint)},{BoolToString(item.HasValueTypeConstraint)},{BoolToString(item.HasUnmanagedTypeConstraint)},{BoolToString(item.HasNotNullConstraint)},{BoolToString(item.HasConstructorConstraint)},new System.Type[] {{ {string.Join(",",item.ConstraintTypes.Select(x=>$"typeof({x})"))} }})";
                    typePars.Add(typePar);
                }

                //Arg interface inject

                var implementInvokeInterface = string.Empty;
                var invokeImplement=string.Empty;

                if (!method.IsGenericMethod)
                {
                    var argOutof16 = method.Parameters.Length > 16;
                    var hasReturn = !method.ReturnsVoid;
                    implementInvokeInterface = ",";
                    var unsafeScript = @"
#if NETFRAMEWORK
unsafe
#endif
";
                    if (argOutof16)
                    {
                        if (hasReturn)
                        {
                            implementInvokeInterface += $"StaticReflection.Invoking.IArgsAnyMethod<{targetType},{method.ReturnType}>";
                            invokeImplement = $"public {unsafeScript} ref {method.ReturnType} Invoke({targetType} instance, params object[] inputs)";
                        }
                        else
                        {
                            implementInvokeInterface += $"StaticReflection.Invoking.IVoidArgsAnyMethod<{targetType}>";
                            invokeImplement = $"public {unsafeScript} void Invoke({targetType} instance, params object[] inputs)";
                        }
                    }
                    else if (method.Parameters.Length == 0)
                    {
                        if (hasReturn)
                        {
                            implementInvokeInterface += $"StaticReflection.Invoking.IArgsMethod<{targetType},{method.ReturnType}>";
                            invokeImplement = $"public {unsafeScript} ref {method.ReturnType} Invoke({targetType} instance)";
                        }
                        else
                        {
                            implementInvokeInterface += $"StaticReflection.Invoking.IVoidArgsMethod<{targetType}>";
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
                        implementInvokeInterface += string.Join(",", method.Parameters.Select(x=>x.Type)) + ">";
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
                    var call = $"instance.{method.Name}({string.Join(",",bodyCodes)})";
                    if (hasReturn)
                    {
                        call = $@"
ref {method.ReturnType} result =ref System.Runtime.CompilerServices.Unsafe.AsRef({call});
return ref result;
";
                    }
                    else
                    {
                        call += ";";
                    }
                    invokeImplement += initCodes;
                    invokeImplement += $"\n{call}\n}}\n";
                }

                string CompileArg(string inputType,string inputArg,IParameterSymbol symbol,StringBuilder initCodes)
                {
                    if (symbol.RefKind== RefKind.None)
                    {
                        return $"System.Runtime.CompilerServices.Unsafe.As<{inputType},{symbol.Type}>(ref {inputArg})";
                    }
                    else if (symbol.RefKind== RefKind.Ref)
                    {
                        return $"ref System.Runtime.CompilerServices.Unsafe.As<{inputType},{symbol.Type}>(ref {inputArg})";
                    }
                    else if (symbol.RefKind== RefKind.RefReadOnly)
                    {
                        return $"ref System.Runtime.CompilerServices.Unsafe.As<{inputType},{symbol.Type}>(ref {inputArg})";
                    }
                    else if (symbol.RefKind== RefKind.In)
                    {
                        return $"in System.Runtime.CompilerServices.Unsafe.As<{inputType},{symbol.Type}>(ref {inputArg})";
                    }
                    else if (symbol.RefKind == RefKind.Out)
                    {
                        var argName= "out"+inputArg;
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
    {visibility} class {ssr} : StaticReflection.IMethodDefine{implementInvokeInterface}
    {{
        public static readonly {ssr} Instance = new {ssr}();

        private {ssr}(){{ }}

        public System.Type DeclareType {{ get; }} = typeof({targetType});

        public System.String Name {{ get; }} = ""{method.Name}"";

        public System.String MetadataName {{ get; }} = ""{method.MetadataName}"";

        public System.Boolean IsVirtual {{ get; }} = {BoolToString(method.IsVirtual)};

        public System.Boolean IsStatic {{ get; }} = {BoolToString(method.IsStatic)};

        public System.Boolean IsOverride {{ get; }} = {BoolToString(method.IsOverride)};

        public System.Boolean IsAbstract {{ get; }} = {BoolToString(method.IsAbstract)};

        public System.Boolean IsSealed {{ get; }} = {BoolToString(method.IsSealed)};

        public System.Boolean IsDefinition {{ get; }} = {BoolToString(method.IsDefinition)};

        public System.Boolean IsExtern {{ get; }} = {BoolToString(method.IsExtern)};

        public System.Boolean IsImplicitlyDeclared {{ get; }} = {BoolToString(method.IsImplicitlyDeclared)};
        
        public System.Boolean CanBeReferencedByName {{ get; }} = {BoolToString(method.CanBeReferencedByName)};
        
        public System.Boolean IsPublic {{ get; }} = {BoolToString(method.DeclaredAccessibility == Accessibility.Public)};
        
        public System.Boolean IsPrivate {{ get; }} = {BoolToString(method.DeclaredAccessibility == Accessibility.Private)};
        
        public System.Boolean IsProtected {{ get; }} = {BoolToString(method.DeclaredAccessibility == Accessibility.Protected || method.DeclaredAccessibility == Accessibility.ProtectedAndInternal)};
        
        public System.Boolean IsInternal {{ get; }} = {BoolToString(method.DeclaredAccessibility == Accessibility.Internal || method.DeclaredAccessibility == Accessibility.ProtectedAndInternal)};

        public System.Boolean ReturnsByRef {{ get; }} = {BoolToString(method.ReturnsByRef)};        
 
        public System.Boolean ReturnsByRefReadonly {{ get; }} = {BoolToString(method.ReturnsByRefReadonly)};        

        public System.Type ReturnType {{ get; }} = typeof({method.ReturnType});     

        public System.Collections.Generic.IReadOnlyList<System.Type> ArgumentTypes {{ get; }} = new System.Type[]{{ {string.Join(",",method.Parameters.Select(x=>$"typeof({x.Type})"))} }};        
         
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
        
        public System.Collections.Generic.IReadOnlyList<System.Attribute> Attributes {{ get; }} = new System.Attribute[] {{ {string.Join(",", attributeStrs)} }};

        public System.Collections.Generic.IReadOnlyList<StaticReflection.ITypeArgumentDefine> TypeArguments {{ get; }} = new StaticReflection.ITypeArgumentDefine[] {{ {string.Join(",", typePars)} }};
        
        {invokeImplement}
    }}
";
                scriptBuilder.AppendLine(str);
            }

            scriptBuilder.AppendLine();
            scriptBuilder.AppendLine("}");

            var code = FormatCode(scriptBuilder.ToString());
            context.AddSource($"{name}{"MethodsReflection"}.g.cs", code);
        }
    }
}
