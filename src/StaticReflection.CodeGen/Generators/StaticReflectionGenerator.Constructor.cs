using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace StaticReflection.CodeGen.Generators
{
    public partial class StaticReflectionGenerator
    {
        protected string CreateSymbolProperties(ISymbol symbol)
        {
            var attributeStrs = GetAttributeStrings(symbol.GetAttributes());

            return $@"

        public System.String Name {{ get; }} = ""{symbol.Name}"";

        public System.String MetadataName {{ get; }} = ""{symbol.MetadataName}"";

        public System.Boolean IsVirtual {{ get; }} = {BoolToString(symbol.IsVirtual)};

        public System.Boolean IsStatic {{ get; }} = {BoolToString(symbol.IsStatic)};

        public System.Boolean IsOverride {{ get; }} = {BoolToString(symbol.IsOverride)};

        public System.Boolean IsAbstract {{ get; }} = {BoolToString(symbol.IsAbstract)};

        public System.Boolean IsSealed {{ get; }} = {BoolToString(symbol.IsSealed)};

        public System.Boolean IsDefinition {{ get; }} = {BoolToString(symbol.IsDefinition)};

        public System.Boolean IsExtern {{ get; }} = {BoolToString(symbol.IsExtern)};

        public System.Boolean IsImplicitlyDeclared {{ get; }} = {BoolToString(symbol.IsImplicitlyDeclared)};
        
        public System.Boolean CanBeReferencedByName {{ get; }} = {BoolToString(symbol.CanBeReferencedByName)};
        
        public System.Boolean IsPublic {{ get; }} = {BoolToString(symbol.DeclaredAccessibility == Accessibility.Public)};
        
        public System.Boolean IsPrivate {{ get; }} = {BoolToString(symbol.DeclaredAccessibility == Accessibility.Private)};
        
        public System.Boolean IsProtected {{ get; }} = {BoolToString(symbol.DeclaredAccessibility == Accessibility.Protected || symbol.DeclaredAccessibility == Accessibility.ProtectedAndInternal)};
        
        public System.Boolean IsInternal {{ get; }} = {BoolToString(symbol.DeclaredAccessibility == Accessibility.Internal || symbol.DeclaredAccessibility == Accessibility.ProtectedAndInternal)};
         
        public System.Collections.Generic.IReadOnlyList<System.Attribute> Attributes {{ get; }} = new System.Attribute[] {{ {string.Join(",", attributeStrs)} }};
";
        }
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

            var index = 0;
            foreach (var constructor in constructors)
            {
                var ssr = name + index + "CReflection";
                index++;
                var attributeStrs = GetAttributeStrings(constructor.GetAttributes());

                var interfaceName = string.Empty;
                var interfaceImpl = string.Empty;
                if (IsAvaliableVisibility(constructor))
                {

                }
                types.Add(ssr);
                var str = $@"
    {GenHeaders.AttackAttribute}
    {visibility} sealed class {ssr} : StaticReflection.IConstructorDefine{interfaceName}
    {{
        public static readonly {ssr} Instance = new {ssr}();

        private {ssr}(){{ }}

        {CreateSymbolProperties(constructor)}
        {CreateMethodProperies(targetType.ToString(), constructor)}
        {interfaceImpl}
    }}
";
                scriptBuilder.AppendLine(str);
            }

            scriptBuilder.AppendLine();

            var code = FormatCode(scriptBuilder.ToString());
            sourceScript.AppendLine(code);
            return types;
        }
    }
}
