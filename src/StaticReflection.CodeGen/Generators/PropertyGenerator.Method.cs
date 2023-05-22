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

            var scriptBuilder = new StringBuilder($"namespace {nameSpace}");
            scriptBuilder.AppendLine();
            scriptBuilder.AppendLine("{");

            foreach (var method in methods)
            {
                var ssr = name + method.Name + "MReflection";
                var attributeStrs = GetAttributeStrings(method.GetAttributes());
                var str = $@"
    [System.Diagnostics.DebuggerStepThrough]
    [System.Runtime.CompilerServices.CompilerGenerated]
    {visibility} class {ssr} : StaticReflection.IMethodDefine
    {{
        public static readonly {ssr} Instance = new {ssr}();

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
        
        public System.Collections.Generic.IReadOnlyList<Attribute> Attributes {{ get; }} = new System.Attribute[] {{ {string.Join(",", attributeStrs)} }};

    }}
";
                scriptBuilder.AppendLine(str);
            }

            scriptBuilder.AppendLine();
            scriptBuilder.AppendLine("}");
            context.AddSource($"{name}{"MethodsReflection"}.g.cs", scriptBuilder.ToString());
        }
    }
}
