using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace StaticReflection.CodeGen.Generators
{
    public partial class StaticReflectionGenerator
    {
        protected void ExecuteProperty(SourceProductionContext context, GeneratorTransformResult<TypeDeclarationSyntax> node,INamedTypeSymbol targetType)
        {
            var members = targetType.GetMembers();
            var properyies = members.OfType<IPropertySymbol>().Where(x => !x.IsIndexer).ToList();
            if (properyies.Count == 0)
            {
                return;
            }
            var visibility = GetAccessibilityString(targetType.DeclaredAccessibility);

            var nameSpace = targetType.ContainingNamespace.ToString();
            var name = targetType.ToString().Split('.').Last();

            var scriptBuilder = new StringBuilder($"namespace {nameSpace}");
            scriptBuilder.AppendLine();
            scriptBuilder.AppendLine("{");

            foreach (var property in properyies)
            {
                var ssr = name + property.Name + "PReflection";
                var attributeStrs = GetAttributeStrings(property.GetAttributes());
                var getBody = $"throw new System.InvalidOperationException(\"The property {targetType}.{property} is set only\");";
                if (!property.IsWriteOnly)
                {
                    if (property.IsStatic)
                    {
                        getBody = $"return {targetType}.{property.Name};";
                    }
                    else
                    {
                        getBody = $"return instance.{property.Name};";
                    }
                }
                var setBody = $"throw new System.InvalidOperationException(\"The property {targetType}.{property} is read only\");";
                if (!property.IsReadOnly)
                {
                    if (property.IsStatic)
                    {
                        setBody = $"{targetType}.{property.Name} = value;";
                    }
                    else
                    {
                        setBody = $"instance.{property.Name} = value;";
                    }
                }
                var str = $@"
    [System.Diagnostics.DebuggerStepThrough]
    [System.Runtime.CompilerServices.CompilerGenerated]
    {visibility} class {ssr}
    {{
        public static readonly {ssr} Instance = new {ssr}();

        public System.Type DeclareType {{ get; }} = typeof({targetType});

        public System.String Name {{ get; }} = ""{property.Name}"";

        public System.String MetadataName {{ get; }} = ""{property.MetadataName}"";

        public System.Boolean IsVirtual {{ get; }} = {BoolToString(property.IsVirtual)};

        public System.Boolean IsStatic {{ get; }} = {BoolToString(property.IsStatic)};

        public System.Boolean IsOverride {{ get; }} = {BoolToString(property.IsOverride)};

        public System.Boolean IsAbstract {{ get; }} = {BoolToString(property.IsAbstract)};

        public System.Boolean IsSealed {{ get; }} = {BoolToString(property.IsSealed)};

        public System.Boolean IsDefinition {{ get; }} = {BoolToString(property.IsDefinition)};

        public System.Boolean IsExtern {{ get; }} = {BoolToString(property.IsExtern)};

        public System.Boolean IsImplicitlyDeclared {{ get; }} = {BoolToString(property.IsImplicitlyDeclared)};
        
        public System.Boolean CanBeReferencedByName {{ get; }} = {BoolToString(property.CanBeReferencedByName)};
        
        public System.Boolean IsPublic {{ get; }} = {BoolToString(property.DeclaredAccessibility == Accessibility.Public)};
        
        public System.Boolean IsPrivate {{ get; }} = {BoolToString(property.DeclaredAccessibility == Accessibility.Private)};
        
        public System.Boolean IsProtected {{ get; }} = {BoolToString(property.DeclaredAccessibility == Accessibility.Protected || property.DeclaredAccessibility == Accessibility.ProtectedAndInternal)};
        
        public System.Boolean IsInternal {{ get; }} = {BoolToString(property.DeclaredAccessibility == Accessibility.Internal || property.DeclaredAccessibility == Accessibility.ProtectedAndInternal)};

        public System.Type PropertyType {{ get; }} = typeof({property.Type.ToString().TrimEnd('?')});

        public System.Boolean CanRead {{ get; }} = {BoolToString(property.GetMethod != null)};

        public System.Boolean CanWrite {{ get; }} = {BoolToString(property.SetMethod != null)};        
        
        public System.Boolean IsRequired {{ get; }} = {BoolToString(property.IsRequired)};  

        public System.Boolean IsWithEvents {{ get; }} = {BoolToString(property.IsWithEvents)};        

        public System.Boolean ReturnsByRef {{ get; }} = {BoolToString(property.ReturnsByRef)};        
 
        public System.Boolean ReturnsByRefReadonly {{ get; }} = {BoolToString(property.ReturnsByRefReadonly)};        
         
        public System.Collections.Generic.IReadOnlyList<System.Attribute> GetterAttributes {{ get; }} = new System.Attribute[] {{ {string.Join(",", GetAttributeStrings(property.GetMethod?.GetAttributes()))} }};  
        
        public System.Collections.Generic.IReadOnlyList<System.Attribute> SetterAttributes {{ get; }} = new System.Attribute[] {{ {string.Join(",", GetAttributeStrings(property.SetMethod?.GetAttributes()))} }};  

        public System.Collections.Generic.IReadOnlyList<System.Attribute> Attributes {{ get; }} = new System.Attribute[] {{ {string.Join(",", attributeStrs)} }};

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public {property.Type} GetValue({name} instance)
        {{
            {getBody}
        }}
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetValue({name} instance,{property.Type} value)
        {{
            {setBody}
        }}
    }}
";
                scriptBuilder.AppendLine(str);
            }
            scriptBuilder.AppendLine();
            scriptBuilder.AppendLine("}");
            context.AddSource($"{name}{"PropertiesReflection"}.g.cs", scriptBuilder.ToString());

        }
    }
}
