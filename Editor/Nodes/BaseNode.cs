using System;
using Unity.GraphToolkit.Editor;

namespace Dialect.Editor.Nodes
{
    [Serializable]
    internal abstract class BaseNode : Node
    {
        public const string EXECUTION_PORT_DEFAULT_NAME = "ExecutionPort";
        public const string INPUT_DISPLAY_NAME = "In";
        public const string OUTPUT_DISPLAY_NAME = "Out";
        
        public void AddInputContextPort<T>(IPortDefinitionContext ctx, string portName = null, string displayName = null)
        {
            ctx.AddInputPort<T>(portName)
                .WithDisplayName(displayName)
                .WithConnectorUI(PortConnectorUI.Circle)
                .Build();
        }
        
        public void AddOutputContextPort<T>(IPortDefinitionContext ctx, string portName = null, string displayName = null)
        {
            ctx.AddOutputPort<T>(portName)
                .WithDisplayName(displayName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
        
        public void AddInputContextPort(IPortDefinitionContext ctx, string portName = null, string displayName = null)
        {
            ctx.AddInputPort(portName)
                .WithDisplayName(displayName)
                .WithConnectorUI(PortConnectorUI.Circle)
                .Build();
        }
        
        public void AddOutputContextPort(IPortDefinitionContext ctx,string portName = null, string displayName = null)
        {
            ctx.AddOutputPort(portName)
                .WithDisplayName(displayName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
}
