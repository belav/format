﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Tools.Formatters;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Tools.Tests.Formatters
{
    public class UnnecessaryImportsFormatterTests : CSharpFormatterTests
    {
        private const string RemoveUnnecessaryImportDiagnosticKey =
            AnalyzerOptionsExtensions.DotnetDiagnosticPrefix + "." + UnnecessaryImportsFormatter.IDE0005 + "." + AnalyzerOptionsExtensions.SeveritySuffix;
        private const string RemoveUnnecessaryImportCategoryKey =
            AnalyzerOptionsExtensions.DotnetAnalyzerDiagnosticPrefix + "." + AnalyzerOptionsExtensions.CategoryPrefix + "-" + UnnecessaryImportsFormatter.Style + "." + AnalyzerOptionsExtensions.SeveritySuffix;

        private protected override ICodeFormatter Formatter =>
            new UnnecessaryImportsFormatter();

        public UnnecessaryImportsFormatterTests(ITestOutputHelper output)
        {
            TestOutputHelper = output;
        }

        [Fact]
        public async Task WhenNotFixingCodeSyle_AndHasUnusedImports_NoChange()
        {
            var code = @"using System;

class C
{
}";

            var editorConfig = new Dictionary<string, string>();

            await AssertCodeUnchangedAsync(
                code,
                editorConfig,
                fixCategory: FixCategory.Whitespace,
                codeStyleSeverity: DiagnosticSeverity.Info
            );
        }

        [Fact]
        public async Task WhenIDE0005NotConfigured_AndHasUnusedImports_NoChange()
        {
            var code = @"using System;

class C
{
}";

            var editorConfig = new Dictionary<string, string>();

            await AssertCodeUnchangedAsync(
                code,
                editorConfig,
                fixCategory: FixCategory.Whitespace
                | FixCategory.CodeStyle,
                codeStyleSeverity: DiagnosticSeverity.Info
            );
        }

        [Theory]
        [InlineData(RemoveUnnecessaryImportDiagnosticKey, Severity.Warning)]
        [InlineData(RemoveUnnecessaryImportDiagnosticKey, Severity.Info)]
        [InlineData(RemoveUnnecessaryImportCategoryKey, Severity.Warning)]
        [InlineData(RemoveUnnecessaryImportCategoryKey, Severity.Info)]
        [InlineData(
                AnalyzerOptionsExtensions.DotnetAnalyzerDiagnosticSeverityKey,
                Severity.Warning)]
        [InlineData(
                AnalyzerOptionsExtensions.DotnetAnalyzerDiagnosticSeverityKey,
                Severity.Info)]
        public async Task WhenIDE0005SeverityLowerThanFixSeverity_AndHasUnusedImports_NoChange(
            string key,
            string severity)
        {
            var code = @"using System;

class C
{
}";

            var editorConfig = new Dictionary<string, string>()
            {
                [key] = severity
            };

            await AssertCodeUnchangedAsync(
                code,
                editorConfig,
                fixCategory: FixCategory.Whitespace
                | FixCategory.CodeStyle,
                codeStyleSeverity: DiagnosticSeverity.Error
            );
        }

        [Theory]
        [InlineData(RemoveUnnecessaryImportDiagnosticKey, Severity.Warning)]
        [InlineData(RemoveUnnecessaryImportDiagnosticKey, Severity.Error)]
        [InlineData(RemoveUnnecessaryImportCategoryKey, Severity.Warning)]
        [InlineData(RemoveUnnecessaryImportCategoryKey, Severity.Error)]
        [InlineData(
                AnalyzerOptionsExtensions.DotnetAnalyzerDiagnosticSeverityKey,
                Severity.Warning)]
        [InlineData(
                AnalyzerOptionsExtensions.DotnetAnalyzerDiagnosticSeverityKey,
                Severity.Error)]
        public async Task WhenIDE0005SeverityEqualOrGreaterThanFixSeverity_AndHasUnusedImports_ImportRemoved(
            string key,
            string severity)
        {
            var testCode = @"using System;

class C
{
}";

            var expectedCode = @"class C
{
}";

            var editorConfig = new Dictionary<string, string>()
            {
                [key] = severity
            };

            await AssertCodeChangedAsync(
                testCode,
                expectedCode,
                editorConfig,
                fixCategory: FixCategory.Whitespace
                | FixCategory.CodeStyle,
                codeStyleSeverity: DiagnosticSeverity.Warning
            );
        }
    }
}
