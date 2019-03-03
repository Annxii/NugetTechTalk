using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;
using NSBContractAnalyzer;
using AirSupport.TechTalk.NSBContractAnalyzer;

namespace NSBContractAnalyzer.Test
{
    [TestClass]
    public class UnitTest : DiagnosticVerifier
    {
        [TestMethod]
        public void No_check_on_empty_code()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void Error_reported_for_missing_event_namespace()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace Airsupport.ImportantSystem.Contract
    {
        public class ImportantEvent
        {   
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = CorrectNamespaceAnalyzer.DiagnosticId,
                Message = "The type 'ImportantEvent' needs to be in the namespace 'Events'",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 11, 22)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new CorrectNamespaceAnalyzer();
        }
    }
}
