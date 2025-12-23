using System;
using UnityEngine.Localization;

namespace Dialect.Editor.Nodes
{
    [Serializable]
    internal class DialogueNode : BaseNode
    {
        const string SPEAKER_NAME_PORT = "SpeakerName";
        const string DIALOGUE_TEXT_PORT = "DialogueText";
        const string SPEAKER_NAME_DISPLAY = "Speaker Name";
        const string DIALOGUE_TEXT_DISPLAY = "Dialogue Text";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputContextPort(context, EXECUTION_PORT_DEFAULT_NAME, INPUT_DISPLAY_NAME);
            AddOutputContextPort(context, EXECUTION_PORT_DEFAULT_NAME, OUTPUT_DISPLAY_NAME);

            context.AddInputPort<string>(SPEAKER_NAME_PORT)
                .WithDisplayName(SPEAKER_NAME_DISPLAY)
                .Build();

            context.AddInputPort<string>(DIALOGUE_TEXT_PORT)
                .WithDisplayName(DIALOGUE_TEXT_DISPLAY)
                .Build();
        }
    }
}