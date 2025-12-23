using Dialect.Core;

namespace Dialect.Editor.Nodes
{
    internal interface IConvertibleToRuntime
    {
        RuntimeNode CreateRuntimeNode();
    }
}