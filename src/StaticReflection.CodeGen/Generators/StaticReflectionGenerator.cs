using Microsoft.CodeAnalysis;

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

            var assemblyProvider = context.SyntaxProvider
                .ForAttributeWithMetadataName(StaticReflectionAssemblyAttributeConsts.Name, PredicateAssembly, TransformAssembly)
                .Where(x => x != null);
            context.RegisterImplementationSourceOutput(assemblyProvider, ExecuteAssembly!);
        }
        protected void Execute(SourceProductionContext context, GeneratorTransformResult<ISymbol?>? node)
        {
            var parser=new Parser(context, node!);
            parser.Execute();
        }
        protected void ExecuteAssembly(SourceProductionContext context, GeneratorTransformResult<ISymbol?>? node)
        {
            var parser = new Parser(context, node!);
            parser.ExecuteAssembly();
        }
        protected GeneratorTransformResult<ISymbol?>? Transform(GeneratorAttributeSyntaxContext context, CancellationToken token)
        {
            return new GeneratorTransformResult<ISymbol?>(context.TargetSymbol, context);
        }
        protected GeneratorTransformResult<ISymbol?>? TransformAssembly(GeneratorAttributeSyntaxContext context, CancellationToken token)
        {
            return new GeneratorTransformResult<ISymbol?>(context.TargetSymbol, context);
        }
        protected bool Predicate(SyntaxNode node, CancellationToken token)
        {
            return true;
        }
        protected bool PredicateAssembly(SyntaxNode node, CancellationToken token)
        {
            return true;
        }
    }

}
