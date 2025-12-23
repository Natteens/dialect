using System;
using Dialect.Actions;
using Dialect.Core;
using Dialect.Editor.Utils;
using Dialect.Nodes;

namespace Dialect.Editor.Nodes
{
    [Serializable]
    internal class ActionNode : BaseNode, IConvertibleToRuntime
    {
        const string ACTION_PORT = "action";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputContextPort(context, EXECUTION_PORT_DEFAULT_NAME, INPUT_DISPLAY_NAME);
            AddInputContextPort<DialectAction>(context, ACTION_PORT, "Action");
            AddOutputContextPort(context, EXECUTION_PORT_DEFAULT_NAME, OUTPUT_DISPLAY_NAME);
        }

        public RuntimeNode CreateRuntimeNode()
        {
            var actionPort = GetInputPortByName(ACTION_PORT);
            var action = NodeUtility.GetInputPortValue<DialectAction>(actionPort);
            
            return new ActionRuntimeNode
            {
                action = action
            };
        }
    }
}