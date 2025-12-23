using System;
using Dialect.Actions;

namespace Dialect.Editor.Nodes
{
    [Serializable]
    internal class ActionNode : BaseNode
    {
        const string ACTION_PORT = "action";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputContextPort(context, EXECUTION_PORT_DEFAULT_NAME, INPUT_DISPLAY_NAME);
            
            context.AddInputPort<DialectAction>(ACTION_PORT)
                .WithDisplayName("Action")
                .Build();
            
            AddOutputContextPort(context, EXECUTION_PORT_DEFAULT_NAME, OUTPUT_DISPLAY_NAME);
        }
    }
}