using Microsoft.CodeAnalysis;

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
    }
}
