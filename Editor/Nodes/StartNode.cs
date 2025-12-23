using System;

namespace Dialect.Editor.Nodes
{
    [Serializable]
    internal class StartNode : BaseNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddOutputContextPort(context, EXECUTION_PORT_DEFAULT_NAME, OUTPUT_DISPLAY_NAME);
        }
    }
}