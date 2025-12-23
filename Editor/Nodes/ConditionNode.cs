using System;
using Dialect.Conditions;
using Dialect.Core;
using Dialect.Editor.Utils;
using Dialect.Nodes;

namespace Dialect.Editor.Nodes
{
    [Serializable]
    internal class ConditionNode : BaseNode, IConvertibleToRuntime
    {
        const string CONDITION_PORT = "condition";
        const string TRUE_PORT = "true";
        const string FALSE_PORT = "false";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputContextPort(context, EXECUTION_PORT_DEFAULT_NAME, INPUT_DISPLAY_NAME);
            AddInputContextPort<DialectCondition>(context, CONDITION_PORT, "Condition");
            AddOutputContextPort(context, TRUE_PORT, "True");
            AddOutputContextPort(context, FALSE_PORT, "False");
        }

        public RuntimeNode CreateRuntimeNode()
        {
            var conditionPort = GetInputPortByName(CONDITION_PORT);
            var condition = NodeUtility.GetInputPortValue<DialectCondition>(conditionPort);
            
            return new ConditionRuntimeNode
            {
                condition = condition
            };
        }
    }
}