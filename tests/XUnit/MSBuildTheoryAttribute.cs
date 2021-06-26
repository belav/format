﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.
using System;
using Xunit;
using Xunit.Sdk;

#nullable enable
namespace Microsoft.CodeAnalysis.Tools.Tests.XUnit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer(
        "Microsoft.CodeAnalysis.Tools.Tests.XUnit.MSBuildTheoryDiscoverer",
        "dotnet-format.UnitTests"
    )]
    public sealed class MSBuildTheoryAttribute : TheoryAttribute { }
}
