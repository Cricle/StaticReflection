using Microsoft.CodeAnalysis;
using System.Text;

namespace StaticReflection.CodeGen.Generators
{
    public partial class StaticReflectionGenerator
    {
        protected List<string> ExecuteFields(SourceProductionContext context, GeneratorTransformResult<ISymbol> node, INamedTypeSymbol targetType)
        {
            var members = targetType.GetMembers();
            var fields = members.OfType<IFieldSymbol>().Where(x => IsAvaliableVisibility(x)).ToList();
            if (fields.Count == 0)
            {
                return new List<string>(0);
            }
            var visibility = GetAccessibilityString(targetType.DeclaredAccessibility);

            var nameSpace = targetType.ContainingNamespace.ToString();
            var name = targetType.ToString().Split('.').Last();

            var scriptBuilder = new StringBuilder();

            var types = new List<string>();

            foreach (var field in fields)
            {
                var ssr = name + field.Name + "FReflection";
                var attributeStrs = GetAttributeStrings(node.SyntaxContext.SemanticModel, field.GetAttributes());
                var avaVisi = IsAvaliableVisibility(field);
                types.Add(ssr);

                var getBody = $"throw new System.InvalidOperationException(\"The property {targetType}.{field} is set only\");";
                if (avaVisi)
                {
                    if (field.IsStatic)
                    {
                        getBody = $"return {targetType}.{field.Name};";
                    }
                    else
                    {
                        getBody = $"return instance.{field.Name};";
                    }
                }
                var setBody = $"throw new System.InvalidOperationException(\"The property {targetType}.{field} is read only\");";
                if (!field.IsConst && avaVisi)
                {
                    if (field.IsStatic)
                    {
                        setBody = $"{targetType}.{field.Name} = value;";
                    }
                    else
                    {
                        setBody = $"instance.{field.Name} = value;";
                    }
                }

                var str = $@"
    {GenHeaders.AttackAttribute}
    {visibility} sealed class {ssr}:IFieldDefine,StaticReflection.IMemberInvokeDefine<{targetType},{field.Type}>,StaticReflection.IMemberAnonymousInvokeDefine
    {{
        public static readonly {ssr} Instance = new {ssr}();

        public System.Type DeclareType {{ get; }} = typeof({targetType});

        public System.String Name {{ get; }} = ""{field.Name}"";

        public System.String MetadataName {{ get; }} = ""{field.MetadataName}"";

        public System.Boolean IsVirtual {{ get; }} = {BoolToString(field.IsVirtual)};

        public System.Boolean IsStatic {{ get; }} = {BoolToString(field.IsStatic)};

        public System.Boolean IsOverride {{ get; }} = {BoolToString(field.IsOverride)};

        public System.Boolean IsAbstract {{ get; }} = {BoolToString(field.IsAbstract)};

        public System.Boolean IsSealed {{ get; }} = {BoolToString(field.IsSealed)};

        public System.Boolean IsDefinition {{ get; }} = {BoolToString(field.IsDefinition)};

        public System.Boolean IsExtern {{ get; }} = {BoolToString(field.IsExtern)};

        public System.Boolean IsImplicitlyDeclared {{ get; }} = {BoolToString(field.IsImplicitlyDeclared)};
        
        public System.Boolean CanBeReferencedByName {{ get; }} = {BoolToString(field.CanBeReferencedByName)};
        
        public System.Boolean IsPublic {{ get; }} = {BoolToString(field.DeclaredAccessibility == Accessibility.Public)};
        
        public System.Boolean IsPrivate {{ get; }} = {BoolToString(field.DeclaredAccessibility == Accessibility.Private)};
        
        public System.Boolean IsProtected {{ get; }} = {BoolToString(field.DeclaredAccessibility == Accessibility.Protected || field.DeclaredAccessibility == Accessibility.ProtectedAndInternal)};
        
        public System.Boolean IsInternal {{ get; }} = {BoolToString(field.DeclaredAccessibility == Accessibility.Internal || field.DeclaredAccessibility == Accessibility.ProtectedAndInternal)};

        public System.Type PropertyType {{ get; }} = typeof({field.Type.ToString().TrimEnd('?')});

        public System.Boolean IsConst {{ get; }} = {BoolToString(field.IsConst)};

        public System.Boolean IsReadOnly {{ get; }} = {BoolToString(field.IsReadOnly)};

        public System.Boolean IsVolatile {{ get; }} = {BoolToString(field.IsVolatile)};

        public System.Boolean IsRequired {{ get; }} = {BoolToString(field.IsRequired)};

        public System.Boolean IsFixedSizeBuffer {{ get; }} = {BoolToString(field.IsFixedSizeBuffer)};

        public System.Int32 FixedSize {{ get; }} = {field.FixedSize};

        public System.Boolean HasConstantValue {{ get; }} = {BoolToString(field.HasConstantValue)};

        public System.Boolean IsExplicitlyNamedTupleElement {{ get; }} = {BoolToString(field.IsExplicitlyNamedTupleElement)};

        public StaticReflection.StaticRefKind RefKind {{ get; }} = StaticReflection.StaticRefKind.{Enum.GetName(typeof(RefKind), field.RefKind)};

        public System.Collections.Generic.IReadOnlyList<System.Attribute> Attributes {{ get; }} = new System.Attribute[] {{ {string.Join(",", attributeStrs)} }};

        
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public {field.Type} GetValue({name} instance)
        {{
            {getBody}
        }}
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetValue({name} instance,{field.Type} value)
        {{
            {setBody}
        }}
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetValueAnonymous(object instance, object value)
        {{
            SetValue(({targetType})instance,({field.Type})value);
        }}
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public object GetValueAnonymous(object instance)
        {{
            return ({field.Type})GetValue(({targetType})instance);
        }}
    }}
";
                scriptBuilder.AppendLine(str);

            }
            scriptBuilder.AppendLine();
            sourceScript.AppendLine(scriptBuilder.ToString());

            return types;
        }
    }
}
