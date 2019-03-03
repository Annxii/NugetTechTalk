using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AirSupport.TechTalk.NSBContractAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CorrectNamespaceAnalyzer : DiagnosticAnalyzer
    {
        private readonly static IReadOnlyDictionary<string, string> suffixNamespaceMap = new Dictionary<string, string>
        {
            { "Event", "Events" },
            { "Command", "Commands" }
        };

        private readonly static Regex nameSuffixRegex = new Regex($"(?<suffix>{string.Join("|", suffixNamespaceMap.Keys)})$");

        public const string DiagnosticId = "AS9001";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId,
            title: "Incorrect namespace",
            messageFormat: "The type '{0}' needs to be in the namespace '{1}'",
            category: "Naming",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "NSB contract elements need to be in the correct namespaces");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            var m = nameSuffixRegex.Match(namedTypeSymbol.Name);
            if(m.Success)
            {
                var expectedNamespace = suffixNamespaceMap[m.Groups["suffix"].Value];
                var ns = namedTypeSymbol.ContainingNamespace?.Name ?? string.Empty;
                if(!expectedNamespace.Equals(ns, StringComparison.Ordinal))
                {
                    var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name, expectedNamespace);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
