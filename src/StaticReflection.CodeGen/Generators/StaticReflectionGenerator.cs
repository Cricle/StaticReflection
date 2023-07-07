using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics;
using System.Text;

namespace StaticReflection.CodeGen.Generators
{
    [Generator]
    public partial class StaticReflectionGenerator : IIncrementalGenerator
    {
        private IncrementalGeneratorInitializationContext context;

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            this.context = context;
            var syntaxProvider = context.SyntaxProvider
                .ForAttributeWithMetadataName(StaticReflectionAttributeConsts.Name, Predicate, Transform)
                .Where(x => x != null);

            context.RegisterSourceOutput(syntaxProvider, Execute!);
            var assemblyProvider = context.SyntaxProvider
                .ForAttributeWithMetadataName(StaticReflectionAssemblyAttributeConsts.Name, PredicateAssembly, TransformAssembly)
                .Where(x => x != null);
            context.RegisterImplementationSourceOutput(assemblyProvider, ExecuteAssembly!);
        }
        protected GeneratorTransformResult<ISymbol?>? Transform(GeneratorAttributeSyntaxContext context, CancellationToken token)
        {
            return new GeneratorTransformResult<ISymbol?>(context.TargetSymbol, context);
        }
        protected GeneratorTransformResult<ISymbol?>? TransformAssembly(GeneratorAttributeSyntaxContext context, CancellationToken token)
        {
            return new GeneratorTransformResult<ISymbol?>(context.TargetSymbol, context);
        }
        protected bool Predicate(SyntaxNode node, CancellationToken token)
        {
            return true;
        }
        protected bool PredicateAssembly(SyntaxNode node, CancellationToken token)
        {
            return true;
        }

        private bool IsAvaliableVisibility(ISymbol symbol)
        {
            return symbol.DeclaredAccessibility == Accessibility.Public || symbol.DeclaredAccessibility == Accessibility.Internal || symbol.DeclaredAccessibility == Accessibility.ProtectedAndInternal;
        }
        protected virtual string GetAccessibilityString(Accessibility accessibility)
        {
            if (accessibility == Accessibility.Private)
            {
                return "private";
            }
            if (accessibility == Accessibility.ProtectedAndInternal)
            {
                return "protected internal";
            }
            if (accessibility == Accessibility.Protected)
            {
                return "protected";
            }
            if (accessibility == Accessibility.Internal)
            {
                return "internal";
            }
            if (accessibility == Accessibility.Public)
            {
                return "public";
            }
            return string.Empty;
        }
        private bool IsAutoGen(ISymbol symbol)
        {
            return symbol.GetAttributes()
                    .Any(x => x.AttributeClass?.ToString() == typeof(GeneratorAttribute).FullName);
        }
        protected List<string> GetAttributeStrings(IEnumerable<AttributeData>? attributes)
        {
            if (attributes == null || !attributes.Any())
            {
                return new List<string>(0);
            }
            var attributeStrs = new List<string>();

            foreach (var attr in attributes)
            {
                var attrStr = $"new {attr.AttributeClass}({string.Join(",", attr.ConstructorArguments.Where(x => !x.IsNull).Select(x => x.ToCSharpString()))}) {{ {string.Join(",", attr.NamedArguments.Select(x => $"{x.Key}={x.Value.ToCSharpString()}"))} }}";
                attributeStrs.Add(attrStr);
            }
            return attributeStrs;
        }
        protected string BoolToString(bool b)
        {
            return b ? "true" : "false";
        }
        protected void ExecuteAssembly(SourceProductionContext context, GeneratorTransformResult<ISymbol> node)
        {
            var targetType = (INamedTypeSymbol)node.SyntaxContext.TargetSymbol;
            var visibility = GetAccessibilityString(targetType.DeclaredAccessibility);
            var nameSpace = targetType.ContainingNamespace.ToString();
            var name = targetType.Name;
            var staticKeyWorld = targetType.IsStatic?"static ":string.Empty;
            var withDefault = string.Empty;
            var attr = targetType.GetAttributes().FirstOrDefault(x => x.AttributeClass?.ToString() == StaticReflectionAssemblyAttributeConsts.Name);
            if (attr!=null&&
                (attr.NamedArguments.FirstOrDefault(x=>x.Key==StaticReflectionAssemblyAttributeConsts.WithDefaultName).Value.Value is bool b&&b||
                !attr.NamedArguments.Any(x=>x.Key== StaticReflectionAssemblyAttributeConsts.WithDefaultName)))
            {
                withDefault = $"public static {name} Default {{ get; }} = new {name}();";
            }

            var assemblySymbol = targetType.ContainingAssembly;
            var assemblyIdentity = assemblySymbol.Identity;
            var assemblyNameSpace = assemblySymbol.GlobalNamespace;

            var moduleDefines=new Dictionary<string, string>();

            foreach (var item in assemblySymbol.Modules)
            {
                var moduleName = $"{item.Name.Replace(".","_")}Module";
                var script = $@"

class {moduleName}:StaticReflection.IModuleIdentity
{{
    {CreateSymbolProperties(item)}

    public System.Type DeclareType {{ get; }} = null;
    
    public StaticReflection.INameSpaceIdentity GlobalNamespace {{ get; }} = {CreateNameSpaceIdentity(assemblyNameSpace)};

    public System.Collections.Generic.IReadOnlyList<StaticReflection.IAssemblyIdentity> ReferencedAssemblies {{ get; }} = new StaticReflection.IAssemblyIdentity[]{{ {string.Join(",",item.ReferencedAssemblies.Select(CreateAssemblyIdentity))} }};

    public System.Collections.Generic.IReadOnlyList<System.String> ReferencedAssemblySymbolNames {{ get; }} = new System.String[]{{ {string.Join(",",item.ReferencedAssemblySymbols.Select(x=>$"\"{x.Name}\"")) }}};
}}
";
                moduleDefines[moduleName] = script;
            }

            var str = $@"
{GenHeaders.AutoGenHead}
namespace {nameSpace}
{{
    {GenHeaders.AttackAttribute}
    {visibility} partial {staticKeyWorld}class {name}:StaticReflection.IAssemblyDefine
    {{
        {withDefault}
        
        {string.Join("\n", moduleDefines.Values)}

        {CreateSymbolProperties(assemblySymbol)}

        public System.Boolean IsInteractive {{ get; }} = {BoolToString(assemblySymbol.IsInteractive)};

        public StaticReflection.IAssemblyIdentity Identity {{ get; }} = {CreateAssemblyIdentity(assemblyIdentity)};
        
        public StaticReflection.INameSpaceIdentity GlobalNamespace {{ get; }} = {CreateNameSpaceIdentity(assemblyNameSpace)};
        
        public System.Collections.Generic.IReadOnlyList<StaticReflection.IModuleIdentity> Modules {{ get; }} = new StaticReflection.IModuleIdentity[] {{ {string.Join(",", moduleDefines.Keys.Select(x=>$"new {x}()"))} }};

        public System.Collections.Generic.IReadOnlyList<string> TypeNames {{ get; }}= new System.String[] {{ {string.Join(",",assemblySymbol.TypeNames.Select(x=>$"\"{x}\""))} }};

        public System.Collections.Generic.IReadOnlyList<string> NamespaceNames {{ get; }}= new System.String[] {{ {string.Join(",", assemblySymbol.NamespaceNames.Select(x => $"\"{x}\""))} }};

        public System.Boolean MightContainExtensionMethods {{ get; }} = {BoolToString(assemblySymbol.MightContainExtensionMethods)};

        public System.Type DeclareType {{ get; }} = null;

        public System.Collections.Generic.IReadOnlyList<StaticReflection.ITypeDefine> Types{{ get; }} = new System.Collections.Generic.List<StaticReflection.ITypeDefine>
        {{
            {string.Join(",\n",refTypes.Distinct().Select(x=>x+".Instance"))}
        }};
    }}
}}
"; 
            var code = FormatCode(str);
            context.AddSource($"{name}.g.cs", code);

        }
        private string CreateAssemblyIdentity(AssemblyIdentity assemblyIdentity)
        {
            return $"new StaticReflection.StaticAssemblyIdentity(\"{assemblyIdentity.Name}\",System.Version.Parse(\"{assemblyIdentity.Version}\"),\"{assemblyIdentity.CultureName}\",global::System.Reflection.AssemblyNameFlags.{assemblyIdentity.Flags},global::System.Reflection.AssemblyContentType.{assemblyIdentity.ContentType},{BoolToString(assemblyIdentity.HasPublicKey)},{BytesToString(assemblyIdentity.PublicKey)},{BytesToString(assemblyIdentity.PublicKeyToken)},{BoolToString(assemblyIdentity.IsStrongName)},{BoolToString(assemblyIdentity.IsRetargetable)})";
        }
        private string CreateNameSpaceIdentity(INamespaceSymbol assemblyNameSpace)
        {
            return $"new StaticReflection.StaticNameSpaceIdentity(\"{assemblyNameSpace.Name}\",{BoolToString(assemblyNameSpace.IsGlobalNamespace)},global::StaticReflection.StaticNamespaceKind.{assemblyNameSpace.NamespaceKind})";
        }
        private string BytesToString(IEnumerable<byte> bytes)
        {
            return $"new System.Byte[]{{ {string.Join(",",bytes)} }}";
        }
        protected void Execute(SourceProductionContext context, GeneratorTransformResult<ISymbol> node)
        {
            var targetType = node.SyntaxContext.TargetSymbol;
            if (node.Value != null)
            {
                var processingedEmpty = false;
                var processedType=new HashSet<string>();
                var attributes = targetType.GetAttributes().Where(x => x.AttributeClass?.ToString() == StaticReflectionAttributeConsts.Name).ToList();
                foreach (var attr in attributes)
                {
                    var attrType = (attr.NamedArguments.FirstOrDefault(x => x.Key == StaticReflectionAttributeConsts.TypeName).Value.Value as INamedTypeSymbol)?.OriginalDefinition;
                    targetType = attrType ?? targetType;
                    var newNode = new GeneratorTransformResult<ISymbol>(targetType, node.SyntaxContext);
                    if (attrType != null&& processedType.Add(attrType.ToString()))
                    {
                        ExecuteOne(context, newNode, attr, attrType);
                    }
                    else if(!processingedEmpty)
                    {
                        processingedEmpty = true;
                        ExecuteOne(context, newNode, attr, targetType!);
                    }
                }
            }
        }

        private StringBuilder sourceScript = new StringBuilder();

        private List<string> refTypes = new List<string>();

        protected void ExecuteOne(SourceProductionContext context, GeneratorTransformResult<ISymbol> node, AttributeData data, ISymbol targetType)
        {
            INamedTypeSymbol? nameTypeTarget = targetType as INamedTypeSymbol;
            if (targetType is IPropertySymbol propertySymbol)
            {
                nameTypeTarget = propertySymbol.Type as INamedTypeSymbol;
            }
            else if (targetType is IFieldSymbol fieldSymbol)
            {
                nameTypeTarget = fieldSymbol.Type as INamedTypeSymbol;
            }
            if (nameTypeTarget == null)
            {
                throw new NotSupportedException(targetType.GetType()?.FullName);
            }
            var properties=ExecuteProperty(context, node, nameTypeTarget);
            var methods = ExecuteMethods(context, node, nameTypeTarget);
            var events = ExecuteEvents(context, node, nameTypeTarget);
            var fields = ExecuteFields(context, node, nameTypeTarget);
            var constructors = ExecuteConstructor(context, node, nameTypeTarget);

            var ssr = $"{nameTypeTarget.Name}Reflection";
            var visibility = GetAccessibilityString(targetType.DeclaredAccessibility);

            var nameSpace = targetType.ContainingNamespace.ToString();
            var attributeStrs = GetAttributeStrings(targetType.GetAttributes());
            refTypes.Add(nameSpace+"."+ssr);
            var str = $@"
{GenHeaders.AutoGenHead}
#pragma warning disable CS9082
namespace {nameSpace}
{{
    {GenHeaders.AttackAttribute}
    {visibility} sealed class {ssr} : StaticReflection.ITypeDefine
    {{
        {sourceScript}
        
        public static readonly {ssr} Instance = new {ssr}();

        public System.Type DeclareType {{ get; }} = typeof({nameTypeTarget});

        public System.String Name {{ get; }} = ""{nameTypeTarget.Name}"";

        public System.String MetadataName {{ get; }} = ""{nameTypeTarget.MetadataName}"";

        public System.Boolean IsVirtual {{ get; }} = {BoolToString(nameTypeTarget.IsVirtual)};

        public System.Boolean IsStatic {{ get; }} = {BoolToString(nameTypeTarget.IsStatic)};

        public System.Boolean IsOverride {{ get; }} = {BoolToString(nameTypeTarget.IsOverride)};

        public System.Boolean IsAbstract {{ get; }} = {BoolToString(nameTypeTarget.IsAbstract)};

        public System.Boolean IsSealed {{ get; }} = {BoolToString(nameTypeTarget.IsSealed)};

        public System.Boolean IsDefinition {{ get; }} = {BoolToString(nameTypeTarget.IsDefinition)};

        public System.Boolean IsExtern {{ get; }} = {BoolToString(nameTypeTarget.IsExtern)};

        public System.Boolean IsImplicitlyDeclared {{ get; }} = {BoolToString(nameTypeTarget.IsImplicitlyDeclared)};
        
        public System.Boolean CanBeReferencedByName {{ get; }} = {BoolToString(nameTypeTarget.CanBeReferencedByName)};
        
        public System.Boolean IsPublic {{ get; }} = {BoolToString(nameTypeTarget.DeclaredAccessibility == Accessibility.Public)};
        
        public System.Boolean IsPrivate {{ get; }} = {BoolToString(nameTypeTarget.DeclaredAccessibility == Accessibility.Private)};
        
        public System.Boolean IsProtected {{ get; }} = {BoolToString(nameTypeTarget.DeclaredAccessibility == Accessibility.Protected || nameTypeTarget.DeclaredAccessibility == Accessibility.ProtectedAndInternal)};
        
        public System.Boolean IsInternal {{ get; }} = {BoolToString(nameTypeTarget.DeclaredAccessibility == Accessibility.Internal || nameTypeTarget.DeclaredAccessibility == Accessibility.ProtectedAndInternal)};

        public System.Collections.Generic.IReadOnlyList<System.Attribute> Attributes {{ get; }} = new System.Attribute[] {{ {string.Join(",", attributeStrs)} }};

        public System.Collections.Generic.IReadOnlyList<StaticReflection.IPropertyDefine> Properties {{ get; }} = new StaticReflection.IPropertyDefine[]{{ {string.Join(",", properties.Select(x => $"{x}.Instance"))} }};

        public System.Collections.Generic.IReadOnlyList<StaticReflection.IMethodDefine> Methods {{ get; }} = new StaticReflection.IMethodDefine[]{{ {string.Join(",", methods.Select(x => $"{x}.Instance"))} }};

        public System.Collections.Generic.IReadOnlyList<StaticReflection.IEventDefine> Events {{ get; }} = new StaticReflection.IEventDefine[]{{ {string.Join(",", events.Select(x => $"{x}.Instance"))} }};

        public System.Collections.Generic.IReadOnlyList<StaticReflection.IFieldDefine> Fields {{ get; }} = new StaticReflection.IFieldDefine[]{{ {string.Join(",", fields.Select(x => $"{x}.Instance"))} }};
        
        public System.Collections.Generic.IReadOnlyList<StaticReflection.IConstructorDefine> Constructors {{ get; }} = new StaticReflection.IConstructorDefine[]{{ {string.Join(",", constructors.Select(x => $"{x}.Instance"))} }};
    }}
}}
";
            var code = FormatCode(str);
            context.AddSource($"{nameTypeTarget.Name}{"Reflection"}.g.cs", code);
            sourceScript = new StringBuilder();

        }

        protected string FormatCode(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot();
            return root.NormalizeWhitespace().ToFullString();
        }
    }
}
