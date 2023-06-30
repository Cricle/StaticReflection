using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace StaticReflection.CodeGen.Generators
{
    public partial class StaticReflectionGenerator
    {
        protected List<string> ExecuteConstructor(SourceProductionContext context, GeneratorTransformResult<ISymbol> node, INamedTypeSymbol targetType)
        {
            var members = targetType.GetMembers();
            var constructors = members.OfType<IMethodSymbol>().Where(x =>x.MethodKind== MethodKind.Constructor||x.MethodKind== MethodKind.StaticConstructor).ToList();
            if (constructors.Count == 0)
            {
                return new List<string>(0);
            }
            var visibility = GetAccessibilityString(targetType.DeclaredAccessibility);

            var nameSpace = targetType.ContainingNamespace.ToString();
            var name = targetType.ToString().Split('.').Last();

            var scriptBuilder = new StringBuilder();

            var types = new List<string>();

            foreach (var @event in constructors)
            {
                var ssr = name + @event.Name + "CReflection";
                var attributeStrs = GetAttributeStrings(@event.GetAttributes());

                types.Add(ssr);

                var str = $@"
    [System.Diagnostics.DebuggerStepThrough]
    [System.Runtime.CompilerServices.CompilerGenerated]
    {visibility} sealed class {ssr}:StaticReflection.IConstructorDefine
    {{
        public static readonly {ssr} Instance = new {ssr}();

        private {ssr}(){{ }}

        public System.String Name {{ get; }} = ""{@event.Name}"";

        public System.Type DeclareType {{ get; }} = typeof({targetType});

        public System.String MetadataName {{ get; }} = ""{@event.MetadataName}"";

        public System.Boolean IsVirtual {{ get; }} = {BoolToString(@event.IsVirtual)};

        public System.Boolean IsStatic {{ get; }} = {BoolToString(@event.IsStatic)};

        public System.Boolean IsOverride {{ get; }} = {BoolToString(@event.IsOverride)};

        public System.Boolean IsAbstract {{ get; }} = {BoolToString(@event.IsAbstract)};

        public System.Boolean IsSealed {{ get; }} = {BoolToString(@event.IsSealed)};

        public System.Boolean IsDefinition {{ get; }} = {BoolToString(@event.IsDefinition)};

        public System.Boolean IsExtern {{ get; }} = {BoolToString(@event.IsExtern)};

        public System.Boolean IsImplicitlyDeclared {{ get; }} = {BoolToString(@event.IsImplicitlyDeclared)};
        
        public System.Boolean CanBeReferencedByName {{ get; }} = {BoolToString(@event.CanBeReferencedByName)};
        
        public System.Boolean IsPublic {{ get; }} = {BoolToString(@event.DeclaredAccessibility == Accessibility.Public)};
        
        public System.Boolean IsPrivate {{ get; }} = {BoolToString(@event.DeclaredAccessibility == Accessibility.Private)};
        
        public System.Boolean IsProtected {{ get; }} = {BoolToString(@event.DeclaredAccessibility == Accessibility.Protected || @event.DeclaredAccessibility == Accessibility.ProtectedAndInternal)};
        
        public System.Boolean IsInternal {{ get; }} = {BoolToString(@event.DeclaredAccessibility == Accessibility.Internal || @event.DeclaredAccessibility == Accessibility.ProtectedAndInternal)};
         
        public System.Collections.Generic.IReadOnlyList<System.Attribute> Attributes {{ get; }} = new System.Attribute[] {{ {string.Join(",", attributeStrs)} }};
    }}";
                scriptBuilder.AppendLine(str);
            }

            scriptBuilder.AppendLine();

            var code = FormatCode(scriptBuilder.ToString());
            sourceScript.AppendLine(code);
            return types;
        }
    }
}
