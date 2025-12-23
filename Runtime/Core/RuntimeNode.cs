using System;
using System.Collections.Generic;
using Dialect.Executors;

namespace Dialect.Core
{
    [Serializable]
    public abstract class RuntimeNode
    {
        public List<int> nextNodeIndices = new();
        
        public abstract void Execute(DialectExecutionContext context);
        
        public abstract bool ShouldAutoAdvance();
    }
}