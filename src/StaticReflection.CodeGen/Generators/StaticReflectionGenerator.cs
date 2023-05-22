using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace StaticReflection.CodeGen.Generators
{
    [Generator]
    public partial class StaticReflectionGenerator : IIncrementalGenerator
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
        protected void Execute(SourceProductionContext context, GeneratorTransformResult<TypeDeclarationSyntax> node)
        {
            var sn = node.SyntaxContext.SemanticModel;
            var targetType = sn.GetDeclaredSymbol(node.Value)!;
            //Debugger.Launch();
            var attribute = targetType.GetAttributes().First(x => x.AttributeClass?.ToString() == StaticReflectionAttributeConsts.Name);
            var attributeTypeValue = (Type?)attribute.NamedArguments.FirstOrDefault(x => x.Key == StaticReflectionAttributeConsts.TypeName).Value.Value;
            //var selectType = attributeTypeValue?.FullName ?? targetType.ToString();

            ExecuteProperty(context, node, targetType);
            ExecuteMethods(context, node, targetType);
        }

        protected string FormatCode(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot();
            return root.NormalizeWhitespace().ToFullString();
        }
    }
}
