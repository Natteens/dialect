using System;
using UnityEngine.Localization;

namespace Dialect.Editor.Nodes
{
    [Serializable]
    internal class LocalizedNode : BaseNode
    {
        const string LOCALIZED_INPUT = "localized";
        const string TEXT_OUTPUT = "TextOutput";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputContextPort<LocalizedString>(context,LOCALIZED_INPUT, "Localized String");
            AddOutputContextPort<string>(context, TEXT_OUTPUT, "Text");
        }
    }
}