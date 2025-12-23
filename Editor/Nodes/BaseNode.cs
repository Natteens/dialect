using System;
using Unity.GraphToolkit.Editor;

namespace Dialect.Editor.Nodes
{
    [Serializable]
    internal abstract class BaseNode : Node
    {
        public const string EXECUTION_PORT_DEFAULT_NAME = "ExecutionPort";
        public const string INPUT_DISPLAY_NAME = "Input";
        public const string OUTPUT_DISPLAY_NAME = "Output";
        
        public void AddInputContextPort(IPortDefinitionContext ctx, string portName, string displayName)
        {
            ctx.AddInputPort(portName)
                .WithDisplayName(displayName)
                .WithConnectorUI(PortConnectorUI.Circle)
                .Build();
        }
        
        public void AddOutputContextPort(IPortDefinitionContext ctx, string portName, string displayName)
        {
            ctx.AddOutputPort(portName)
                .WithDisplayName(displayName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
}
