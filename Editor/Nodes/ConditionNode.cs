using System;
using Dialect.Conditions;

namespace Dialect.Editor.Nodes
{
    [Serializable]
    internal class ConditionNode : BaseNode
    {
        const string CONDITION_PORT = "condition";
        const string TRUE_PORT = "true";
        const string FALSE_PORT = "false";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputContextPort(context, EXECUTION_PORT_DEFAULT_NAME, INPUT_DISPLAY_NAME);
            
            context.AddInputPort<DialectCondition>(CONDITION_PORT)
                .WithDisplayName("Condition")
                .Build();
            
            AddOutputContextPort(context, TRUE_PORT, "True");
            AddOutputContextPort(context, FALSE_PORT, "False");
        }
    }
}