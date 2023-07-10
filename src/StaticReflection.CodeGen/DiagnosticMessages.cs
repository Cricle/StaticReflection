using Microsoft.CodeAnalysis;
using StaticReflection.CodeGen.Resx;
using System;
using System.Collections.Generic;
using System.Text;

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
