﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Wyam.Modules.Razor.Microsoft.AspNet.Mvc.Razor.Compilation
{
    /// <summary>
    /// Specifies the contracts for a service that compiles Razor files.
    /// </summary>
    public interface IRazorCompilationService
    {
        /// <summary>
        /// Compiles the razor file located at <paramref name="fileInfo"/>.
        /// </summary>
        /// <param name="fileInfo">A <see cref="RelativeFileInfo"/> instance that represents the file to compile.
        /// </param>
        /// <returns>
        /// A <see cref="CompilationResult"/> that represents the results of parsing and compiling the file.
        /// </returns>
        Type Compile(RelativeFileInfo fileInfo);
    }
}