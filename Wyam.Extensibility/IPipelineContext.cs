﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyam.Extensibility
{
    public interface IPipelineContext
    {
        ITrace Trace { get; }
        IReadOnlyList<IModuleContext> CompletedContexts { get; }

        // This executes the specified modules on the specified input contexts and returns the final result contexts
        // If you pass in null for inputContexts, a new input context with the initial metadata from the engine will be used
        IReadOnlyList<IModuleContext> Execute(IEnumerable<IModule> modules, IEnumerable<IModuleContext> inputContexts);
    }
}