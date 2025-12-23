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
            context.AddInputPort<LocalizedString>(LOCALIZED_INPUT)
                .WithDisplayName("Localized String")
                .Build();

            context.AddOutputPort<string>(TEXT_OUTPUT)
                .WithDisplayName("Text")
                .Build();
        }
    }
}