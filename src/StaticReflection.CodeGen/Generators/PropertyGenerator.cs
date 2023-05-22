using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace StaticReflection.CodeGen.Generators
{
    [Generator]
    public partial class PropertyGenerator : IIncrementalGenerator
    {
        private IncrementalGeneratorInitializationContext context;

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            this.context = context;
            var syntaxProvider = context.SyntaxProvider.CreateSyntaxProvider(Predicate, Transform)
                .Where(x => x != null);
            context.RegisterSourceOutput(syntaxProvider, Execute!);
        }
        protected GeneratorTransformResult<TypeDeclarationSyntax>? Transform(GeneratorSyntaxContext context, CancellationToken token)
        {
            var sn = context.SemanticModel;
            var typeSymbol = sn.GetDeclaredSymbol(context.Node, token);
            if (typeSymbol != null &&
                typeSymbol.GetAttributes().Any(x => x.AttributeClass?.ToString() == StaticReflectionAttributeConsts.Name))
            {
                return new GeneratorTransformResult<TypeDeclarationSyntax>((TypeDeclarationSyntax)context.Node, context);
            }
            return null;
        }
        protected bool Predicate(SyntaxNode node, CancellationToken token)
        {
            if (node is ClassDeclarationSyntax || node is InterfaceDeclarationSyntax)
            {
                var syntax = (TypeDeclarationSyntax)node;
                return syntax.AttributeLists.Count != 0;
            }
            return false;
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
        protected List<string> GetAttributeStrings(IEnumerable<AttributeData> attributes)
        {
            var attributeStrs = new List<string>();

            foreach (var attr in attributes)
            {
                var attrStr = $"new {attr.AttributeClass}({string.Join(",", attr.ConstructorArguments.Where(x => !x.IsNull).Select(x => x.ToCSharpString()))}) {{ {string.Join(",", attr.NamedArguments.Select(x => $"{x.Key}={x.Value.ToCSharpString()}"))} }}";
                attributeStrs.Add(attrStr);
            }
            return attributeStrs;
        }
        protected void Execute(SourceProductionContext context, GeneratorTransformResult<TypeDeclarationSyntax> node)
        {
            var sn = node.SyntaxContext.SemanticModel;
            var targetType = sn.GetDeclaredSymbol(node.Value)!;
            //Debugger.Launch();
            var attribute = targetType.GetAttributes().First(x => x.AttributeClass?.ToString() == StaticReflectionAttributeConsts.Name);
            var attributeTypeValue = (Type?)attribute.NamedArguments.FirstOrDefault(x => x.Key == StaticReflectionAttributeConsts.TypeName).Value.Value;
            //var selectType = attributeTypeValue?.FullName ?? targetType.ToString();

            var members = targetType.GetMembers();
            var properyies = members.OfType<IPropertySymbol>().Where(x => !x.IsIndexer).ToList();
            var visibility = GetAccessibilityString(targetType.DeclaredAccessibility);

            var nameSpace = targetType.ContainingNamespace.ToString();
            var name = targetType.ToString().Split('.').Last();

            foreach (var property in properyies)
            {
                var ssr = name + property.Name + "StaticReflection";
                var attributeStrs = GetAttributeStrings(property.GetAttributes());
                var str = $@"
namespace {nameSpace}
{{
    [System.Diagnostics.DebuggerStepThrough]
    [System.Runtime.CompilerServices.CompilerGenerated]
    {visibility} static class {ssr}
    {{
        public static System.Type DeclareType {{ get; }} = typeof({targetType});

        public static System.String PropertyName {{ get; }} = ""{property.Name}"";

        public static System.Type PropertyType {{ get; }} = typeof({property.Type.ToString().TrimEnd('?')});

        public static bool CanRead {{ get; }} = {(property.IsWriteOnly ? "false" : "true")};

        public static bool CanWrite {{ get; }} = {(property.IsReadOnly ? "false" : "true")};

        public static System.Collections.Generic.IReadOnlyList<Attribute> Attributes {{ get; }} = new Attribute[] {{ {string.Join(",", attributeStrs)} }};

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static {property.Type} GetValue({name} instance)
        {{
            return instance.{property.Name};
        }}
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void SetValue({name} instance,{property.Type} value)
        {{
            instance.{property.Name} = value;
        }}
    }}
    [System.Diagnostics.DebuggerStepThrough]
    [System.Runtime.CompilerServices.CompilerGenerated]
    {visibility} class {name}{property.Name}Reflection : StaticReflection.IPropertyDefine<{targetType},{property.Type}>
    {{

        public static {name}{property.Name}Reflection Instance = new {name}{property.Name}Reflection();
        
        private {name}{property.Name}Reflection() {{ }}
        
        public System.Type DeclareType {{ get; }} = {ssr}.DeclareType;

        public System.String PropertyName {{ get; }} = {ssr}.PropertyName;

        public System.Type PropertyType {{ get; }}={ssr}.PropertyType;
        
        public bool CanRead {{ get; }} = {ssr}.CanRead;

        public bool CanWrite {{ get; }} = {ssr}.CanWrite;
        
        public System.Collections.Generic.IReadOnlyList<Attribute> Attributes {{ get; }} = {ssr}.Attributes;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public {property.Type} GetValue({name} instance)
        {{
            return {ssr}.GetValue(instance);
        }}
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetValue({name} instance,{property.Type} value)
        {{
            {ssr}.SetValue(instance,value);
        }}
    }}
}}
";
                context.AddSource($"{name}{property.Name}{"PropertiesReflection"}.g.cs", str);
            }
            //var tree = CSharpSyntaxTree.ParseText(source);
            //var root = tree.GetRoot();
            //var formattedRoot = root.NormalizeWhitespace().ToFullString();
            //Debugger.Launch();
            //context.AddSource($"{name}{"StaticReflection"}.g.cs", SourceText.From(formattedRoot, Encoding.UTF8));
        }
    }
}
