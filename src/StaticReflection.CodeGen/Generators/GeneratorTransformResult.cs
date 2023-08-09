using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace StaticReflection.CodeGen.Generators
{
    public class GeneratorTransformResult<T>
    {
        public GeneratorTransformResult(T value, GeneratorAttributeSyntaxContext syntaxContext)
        {
            Value = value;
            SyntaxContext = syntaxContext;
        }

        public T Value { get; }

        public GeneratorAttributeSyntaxContext SyntaxContext { get; }

        public IAssemblySymbol AssemblySymbol=> SyntaxContext.SemanticModel.Compilation.Assembly;

        public bool IsAvaliableVisibility(ISymbol symbol)
        {
            if (SymbolEqualityComparer.Default.Equals(SyntaxContext.SemanticModel.Compilation.Assembly, symbol.ContainingAssembly))
            {
                return symbol.DeclaredAccessibility == Accessibility.Public || symbol.DeclaredAccessibility == Accessibility.Internal || symbol.DeclaredAccessibility == Accessibility.ProtectedAndInternal;
            }
            
            return symbol.DeclaredAccessibility == Accessibility.Public;//TODO:InternalsVisibleToAttribute
        }
    }
}
