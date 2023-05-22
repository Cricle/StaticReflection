using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace StaticReflection.CodeGen.Generators
{
    public partial class StaticReflectionGenerator
    {
        protected void ExecuteEvents(SourceProductionContext context, GeneratorTransformResult<TypeDeclarationSyntax> node, INamedTypeSymbol targetType)
        {
            var members = targetType.GetMembers();
            var events = members.OfType<IEventSymbol>().ToList();
            var visibility = GetAccessibilityString(targetType.DeclaredAccessibility);

            var nameSpace = targetType.ContainingNamespace.ToString();
            var name = targetType.ToString().Split('.').Last();

            var scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine($"namespace {nameSpace}");
            scriptBuilder.AppendLine("{");
            foreach (var @event in events)
            {
                var ssr = name + @event.Name + "EReflection";
                var attributeStrs = GetAttributeStrings(@event.GetAttributes());

                var str = $@"
    [System.Diagnostics.DebuggerStepThrough]
    [System.Runtime.CompilerServices.CompilerGenerated]
    {visibility} class {ssr}
    {{
    
    }}";
            }
            scriptBuilder.AppendLine();
            scriptBuilder.AppendLine("}");

            var code = FormatCode(scriptBuilder.ToString());
            context.AddSource($"{name}{"EventsReflection"}.g.cs", code);
        }
    }
}
