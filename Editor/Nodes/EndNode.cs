using System;

namespace Dialect.Editor.Nodes
{
    [Serializable]
    internal class EndNode : BaseNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputContextPort(context, EXECUTION_PORT_DEFAULT_NAME, INPUT_DISPLAY_NAME);
        }
    }
}