using System;
using System.Collections.Generic;
using Dialect.Executors;

namespace Dialect.Core
{
    [Serializable]
    public abstract class DialectRuntimeNode
    {
        public List<int> nextNodeIndices = new();
    }
    
    public interface IDialectExecutor<in TNode> where TNode : DialectRuntimeNode
    {
        void Execute(TNode node, DialectExecutionContext context);
    }
}