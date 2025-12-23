using System;
using Dialect.Core;
using Dialect.Nodes;

namespace Dialect.Editor.Nodes
{
    [Serializable]
    internal class EndNode : BaseNode, IConvertibleToRuntime
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputContextPort(context, EXECUTION_PORT_DEFAULT_NAME, INPUT_DISPLAY_NAME);
        }

        public RuntimeNode CreateRuntimeNode()
        {
            return new EndRuntimeNode();
        }
    }
}