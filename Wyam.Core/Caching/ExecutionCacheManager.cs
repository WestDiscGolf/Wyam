﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyam.Common;
using Wyam.Common.Caching;
using Wyam.Common.Modules;

namespace Wyam.Core.Caching
{
    internal class ExecutionCacheManager
    {
        private readonly ConcurrentDictionary<IModule, ExecutionCache> _executionCaches 
            = new ConcurrentDictionary<IModule, ExecutionCache>();
        private bool _noCache;

        public bool NoCache
        {
            get { return _noCache; }
            set
            {
                _executionCaches.Clear();
                _noCache = value;
            }
        }

        // Creates one if it doesn't exist
        public IExecutionCache Get(IModule module, Engine engine)
        {
            return _noCache
                ? (IExecutionCache) new NoCache()
                : _executionCaches.GetOrAdd(module, new ExecutionCache(engine));
        }

        public void ResetEntryHits()
        {
            foreach (ExecutionCache cache in _executionCaches.Values)
            {
                cache.ResetEntryHits();
            }
        }

        public void ClearUnhitEntries(Engine engine)
        {
            foreach (KeyValuePair<IModule, ExecutionCache> item in _executionCaches)
            {
                int count = item.Value.ClearUnhitEntries().Count;
                engine.Trace.Verbose("Removed {0} stale cache entries for module {1}", count, item.Key.GetType().Name);
            }
        }
    }
}
