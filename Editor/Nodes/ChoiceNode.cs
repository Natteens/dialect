using System;
using UnityEngine.Localization;

namespace Dialect.Editor.Nodes
{
    [Serializable]
    internal class ChoiceNode : BaseNode
    {
        const string PORT_COUNT_OPTION = "portCount";
        const string CHOICE_PORT_PREFIX = "Choice";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(
                name: PORT_COUNT_OPTION,
                dataType: typeof(int)
            ).WithDisplayName(displayName: "Choice Count").WithTooltip("Number of choices").WithDefaultValue(2);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputContextPort(context, EXECUTION_PORT_DEFAULT_NAME, INPUT_DISPLAY_NAME);
            
            int portCount = 2;
            GetNodeOptionByName(PORT_COUNT_OPTION)?.TryGetValue(out portCount);
            portCount = Math.Max(0, Math.Min(portCount, 6));

            for (int i = 0; i < portCount; i++)
            {
                string outPortName = $"{CHOICE_PORT_PREFIX}{i}_Out";
                string inPortName = $"{CHOICE_PORT_PREFIX}{i}_In";

                AddOutputContextPort(context, outPortName, $"Choice {i + 1} Out");
                AddInputContextPort(context, inPortName, $"Choice {i + 1} In");
            }
        }
    }
}