using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Text;
using System.Diagnostics;

namespace StaticReflection.CodeGen.Generators
{
    public partial class StaticReflectionGenerator
    {
        protected List<string> ExecuteEvents(SourceProductionContext context, GeneratorTransformResult<ISymbol> node, INamedTypeSymbol targetType)
        {
            var members = targetType.GetMembers();
            var events = members.OfType<IEventSymbol>().Where(x=>x.Type is INamedTypeSymbol symbol&& IsAvaliableVisibility(x)&&symbol.DelegateInvokeMethod!=null).ToList();
            if (events.Count==0)
            {
                return new List<string>(0);
            }
            var visibility = GetAccessibilityString(targetType.DeclaredAccessibility);

            var nameSpace = targetType.ContainingNamespace.ToString();
            var name = targetType.ToString().Split('.').Last();

            var scriptBuilder = new StringBuilder();

            var types = new List<string>();

            foreach (var @event in events)
            {
                var ssr = name + @event.Name + "EReflection";
                var attributeStrs = GetAttributeStrings(@event.GetAttributes());
                var delegateInvokeMethod = ((INamedTypeSymbol)@event.Type).DelegateInvokeMethod!;
                types.Add(ssr);

                var implInterface = string.Empty;
                var additionClass = string.Empty;
                var implMethods = string.Empty;
                if (IsAvaliableVisibility(@event)&&@event.AddMethod!=null&&@event.RemoveMethod!=null)
                {                
                    var scopeClassName = ssr + "Scope";//Static ?
                    var selectVisit = string.Empty;
                    if (@event.IsStatic)
                    {
                        selectVisit = @event.ContainingType.ToString();
                    }
                    else
                    {
                        selectVisit = $"(({targetType})instance)";
                    }
                    var scopeName = ssr + "Scope";
                    additionClass = $@"
    {GenHeaders.AttackAttribute}
    {visibility} sealed class {scopeName}:StaticReflection.EventTransferScope
    {{
        public {scopeName}(StaticReflection.IEventTransfer root, System.Object? instance)
            :base(root,instance)
        {{
        }}
        protected override void OnStart()
        {{
            {selectVisit}.{@event.Name}+=OnEventRaise;
        }}
        protected override void OnStop()
        {{
            {selectVisit}.{@event.Name}-=OnEventRaise;
        }}
        private void OnEventRaise({string.Join(",", delegateInvokeMethod.Parameters.Select(x => $"{x.Type} {x.Name}"))})
        {{
            OnEventTransfed(new StaticReflection.EventTransferEventArgs(new System.Object[]
            {{
                {string.Join(",", delegateInvokeMethod.Parameters.Select(x => x.Name))}
            }}));
        }}
    }}
";
                    implInterface = "StaticReflection.EventTransfer,";
                    implMethods = $@"
        protected override void OnStart(object instance)
        {{
            {selectVisit}.{@event.Name}+=OnEventRaise;
        }}
        protected override void OnStop(object instance)
        {{
            {selectVisit}.{@event.Name}-=OnEventRaise;
        }}
        private void OnEventRaise({string.Join(",", delegateInvokeMethod.Parameters.Select(x => $"{x.Type} {x.Name}"))})
        {{
            OnEventTransfed(new StaticReflection.EventTransferEventArgs(new System.Object[]
            {{
                {string.Join(",", delegateInvokeMethod.Parameters.Select(x => x.Name))}
            }}));
        }}
        public override StaticReflection.IEventTransferScope CreateScope(System.Object instance)
        {{
            return new {scopeClassName}(this,instance);
        }}
";
                }

                var str = $@"
    {GenHeaders.AttackAttribute}
    {visibility} sealed class {ssr}: {implInterface}StaticReflection.IEventDefine
    {{
        public static readonly {ssr} Instance = new {ssr}();
        
        {additionClass}
        
        private {ssr}(){{ }}

        public System.String Name {{ get; }} = ""{@event.Name}"";

        public System.Type DeclareType {{ get; }} = typeof({targetType.ToString().TrimEnd('?')});

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
        
        public System.Type DelegateType {{ get; }} = typeof({@event.Type.ToString().TrimEnd('?')});

        public System.Boolean IsWindowsRuntimeEvent {{ get; }} = {BoolToString(@event.IsWindowsRuntimeEvent)};    

        public System.Boolean HasAddMethod {{ get; }} = {BoolToString(@event.AddMethod != null)};        

        public System.Boolean HasRemoveMethod {{ get; }} = {BoolToString(@event.RemoveMethod != null)};    

        public System.Boolean HasRaiseMethod {{ get; }} = {BoolToString(@event.RaiseMethod != null)};    

        public System.Boolean IsOverriddenEvent {{ get; }} = {BoolToString(@event.OverriddenEvent != null)};   

        public System.Collections.Generic.IReadOnlyList<System.Attribute> Attributes {{ get; }} = new System.Attribute[] {{ {string.Join(",", attributeStrs)} }};

        public System.Collections.Generic.IReadOnlyList<System.Type> ArgumentTypes {{ get; }} = new System.Type[] {{ {string.Join(",", delegateInvokeMethod.Parameters.Select(x=>$"typeof({x.Type.ToString().TrimEnd('?')})"))} }};
        {implMethods}
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
