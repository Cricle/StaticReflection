using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
using System.Dynamic;
using System.Xml.Linq;

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
        }
        protected GeneratorTransformResult<ISymbol?>? Transform(GeneratorAttributeSyntaxContext context, CancellationToken token)
        {
            return new GeneratorTransformResult<ISymbol?>(context.TargetSymbol, context);
            //var sn = context.SemanticModel;
            //var typeSymbol = sn.GetSymbolInfo(context.TargetNode, token).Symbol;
            //if (typeSymbol != null &&
            //    typeSymbol.GetAttributes().Any(x => x.AttributeClass?.ToString() == StaticReflectionAttributeConsts.Name))
            //{
            //    return new GeneratorTransformResult<MemberDeclarationSyntax>((MemberDeclarationSyntax)context.Node, context);
            //}
            //return null;
        }
        protected bool Predicate(SyntaxNode node, CancellationToken token)
        {
            return true;
            //if (node is MemberDeclarationSyntax)
            //{
            //    return true;
            //}
            //return false;
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
        private static INamedTypeSymbol? GetSymbolFromType(Type type, SemanticModel semanticModel)
        {
            return semanticModel.LookupSymbols(0, name: type.Name).FirstOrDefault() as INamedTypeSymbol;
        }
        protected void Execute(SourceProductionContext context, GeneratorTransformResult<ISymbol> node)
        {
            var sn = node.SyntaxContext.SemanticModel;
            var targetType = node.SyntaxContext.TargetSymbol;
            if (node.Value != null)
            {
                var attribute = targetType.GetAttributes().First(x => x.AttributeClass?.ToString() == StaticReflectionAttributeConsts.Name);
                targetType = (attribute.NamedArguments.FirstOrDefault(x => x.Key == StaticReflectionAttributeConsts.TypeName).Value.Value as INamedTypeSymbol)?.OriginalDefinition?? targetType;
            }

            var nameTypeTarget = (INamedTypeSymbol)targetType!;

            var properties=ExecuteProperty(context, node, nameTypeTarget);
            var methods = ExecuteMethods(context, node, nameTypeTarget);
            var events = ExecuteEvents(context, node, nameTypeTarget);

            var ssr = $"{nameTypeTarget.Name}Reflection";
            var visibility = GetAccessibilityString(targetType.DeclaredAccessibility);

            var nameSpace = targetType.ContainingNamespace.ToString();
            var attributeStrs = GetAttributeStrings(targetType.GetAttributes());

            var str = $@"
namespace {nameSpace}
{{
    {visibility} sealed class {ssr} : ITypeDefine
    {{
        public static readonly {ssr} Instance = new {ssr}();

        public System.Type DeclareType {{ get; }} = typeof({targetType});

        public System.String Name {{ get; }} = ""{targetType.Name}"";

        public System.String MetadataName {{ get; }} = ""{targetType.MetadataName}"";

        public System.Boolean IsVirtual {{ get; }} = {BoolToString(targetType.IsVirtual)};

        public System.Boolean IsStatic {{ get; }} = {BoolToString(targetType.IsStatic)};

        public System.Boolean IsOverride {{ get; }} = {BoolToString(targetType.IsOverride)};

        public System.Boolean IsAbstract {{ get; }} = {BoolToString(targetType.IsAbstract)};

        public System.Boolean IsSealed {{ get; }} = {BoolToString(targetType.IsSealed)};

        public System.Boolean IsDefinition {{ get; }} = {BoolToString(targetType.IsDefinition)};

        public System.Boolean IsExtern {{ get; }} = {BoolToString(targetType.IsExtern)};

        public System.Boolean IsImplicitlyDeclared {{ get; }} = {BoolToString(targetType.IsImplicitlyDeclared)};
        
        public System.Boolean CanBeReferencedByName {{ get; }} = {BoolToString(targetType.CanBeReferencedByName)};
        
        public System.Boolean IsPublic {{ get; }} = {BoolToString(targetType.DeclaredAccessibility == Accessibility.Public)};
        
        public System.Boolean IsPrivate {{ get; }} = {BoolToString(targetType.DeclaredAccessibility == Accessibility.Private)};
        
        public System.Boolean IsProtected {{ get; }} = {BoolToString(targetType.DeclaredAccessibility == Accessibility.Protected || targetType.DeclaredAccessibility == Accessibility.ProtectedAndInternal)};
        
        public System.Boolean IsInternal {{ get; }} = {BoolToString(targetType.DeclaredAccessibility == Accessibility.Internal || targetType.DeclaredAccessibility == Accessibility.ProtectedAndInternal)};

        public System.Collections.Generic.IReadOnlyList<System.Attribute> Attributes {{ get; }} = new System.Attribute[] {{ {string.Join(",", attributeStrs)} }};

        public System.Collections.Generic.IReadOnlyList<StaticReflection.IPropertyDefine> Properties {{ get; }} = new StaticReflection.IPropertyDefine[]{{ {string.Join(",", properties.Select(x => $"{x}.Instance"))} }};

        public System.Collections.Generic.IReadOnlyList<StaticReflection.IMethodDefine> Methods {{ get; }} = new StaticReflection.IMethodDefine[]{{ {string.Join(",", methods.Select(x => $"{x}.Instance"))} }};

        public System.Collections.Generic.IReadOnlyList<StaticReflection.IEventDefine> Events {{ get; }} = new StaticReflection.IEventDefine[]{{ {string.Join(",", events.Select(x => $"{x}.Instance"))} }};
    }}
}}
"; 
            context.AddSource($"{targetType.Name}{"Reflection"}.g.cs", str);

        }

        protected string FormatCode(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot();
            return root.NormalizeWhitespace().ToFullString();
        }
    }
}
