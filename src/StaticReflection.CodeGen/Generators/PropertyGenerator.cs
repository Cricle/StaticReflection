using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
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
            if (accessibility== Accessibility.Private)
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

        protected void Execute(SourceProductionContext context, GeneratorTransformResult<TypeDeclarationSyntax> node)
        {
            var sn = node.SyntaxContext.SemanticModel;
            var targetType = sn.GetDeclaredSymbol(node.Value)!;
            //Debugger.Launch();
            var attribute = targetType.GetAttributes().First(x => x.AttributeClass?.ToString() == StaticReflectionAttributeConsts.Name);
            var attributeTypeValue = (Type?)attribute.NamedArguments.FirstOrDefault(x => x.Key == StaticReflectionAttributeConsts.TypeName).Value.Value;
            //var selectType = attributeTypeValue?.FullName ?? targetType.ToString();

            var members = targetType.GetMembers();
            var properyies = members.OfType<IPropertySymbol>().ToList();
            var visibility = GetAccessibilityString(targetType.DeclaredAccessibility);

            var nameSpace = targetType.ContainingNamespace.ToString();
            var name = targetType.ToString().Split('.').Last();

            foreach (var property in properyies)
            {
                var str= $@"
using StaticReflection;

namespace {nameSpace}
{{
    {visibility} static class {name}{property.Name}StaticReflection
    {{
        public static Type DeclareType {{ get; }} = typeof({targetType});

        public static string PropertyName {{ get; }}=""{property.Name}"";

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
}}
";
                context.AddSource($"{name}{property.Name}{"StaticReflection"}.g.cs", str);

            }
            //var tree = CSharpSyntaxTree.ParseText(source);
            //var root = tree.GetRoot();
            //var formattedRoot = root.NormalizeWhitespace().ToFullString();
            //Debugger.Launch();
            //context.AddSource($"{name}{"StaticReflection"}.g.cs", SourceText.From(formattedRoot, Encoding.UTF8));
        }
    }
}
