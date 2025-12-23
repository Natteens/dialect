using Dialect.Executors;

namespace Dialect.Core
{
    public interface IDialectExecutor<in TNode> where TNode : RuntimeNode
    {
        void Execute(TNode node, DialectExecutionContext context);
    }
}