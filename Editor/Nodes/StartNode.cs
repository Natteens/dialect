using System;
using Dialect.Core;
using Dialect.Nodes;

namespace Dialect.Editor.Nodes
{
    [Serializable]
    internal class StartNode : BaseNode, IConvertibleToRuntime
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddOutputContextPort(context, EXECUTION_PORT_DEFAULT_NAME, OUTPUT_DISPLAY_NAME);
        }

        public RuntimeNode CreateRuntimeNode()
        {
            return new StartRuntimeNode();
        }
    }
}