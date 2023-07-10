using Microsoft.CodeAnalysis;
using StaticReflection.CodeGen.Resx;

namespace StaticReflection.CodeGen
{
    internal static class DiagnosticMessages
    {
        public static readonly DiagnosticDescriptor NoAssemblyFoundDiagnostic = new DiagnosticDescriptor(
            "SRE_0001",
            Strings.SRE_0001_Title,
            Strings.SRE_0001,
            "SRE",
             DiagnosticSeverity.Error,
             true);
    }
}
