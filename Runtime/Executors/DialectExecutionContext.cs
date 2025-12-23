using System;
using Dialect.Core;
using Dialect.Nodes;
using UnityEngine;

namespace Dialect.Executors
{
    public class DialectExecutionContext
    {
        public DialectDirector director;
        public Action<string, string> onDialogueShown;
        public Action<string[]> onChoiceShown;
        public Action onDialogueEnded;
        public object customData;
        public bool lastConditionResult;
    }

    public class DialectStartNodeExecutor : IDialectExecutor<DialectStartRuntimeNode>
    {
        public void Execute(DialectStartRuntimeNode node, DialectExecutionContext context)
        {
            Debug.Log("Starting Dialect Dialogue");
        }
    }

    public class DialogueNodeExecutor : IDialectExecutor<DialogueRuntimeNode>
    {
        public void Execute(DialogueRuntimeNode node, DialectExecutionContext context)
        {
            string speaker = string.Empty;
            string dialogue = string.Empty;
            
            if (node.speakerName != null && !node.speakerName.IsEmpty)
            {
                speaker = node.speakerName.GetLocalizedString() ?? string.Empty;
            }
            
            if (node.dialogueText != null && !node.dialogueText.IsEmpty)
            {
                dialogue = node.dialogueText.GetLocalizedString() ?? string.Empty;
            }
            
            context.onDialogueShown?.Invoke(speaker, dialogue);
            Debug.Log($"[{speaker}]: {dialogue}");
        }
    }

    public class ChoiceNodeExecutor : IDialectExecutor<ChoiceRuntimeNode>
    {
        public void Execute(ChoiceRuntimeNode node, DialectExecutionContext context)
        {
            var choices = new string[node.choiceTexts.Length];
            
            for (int i = 0; i < node.choiceTexts.Length; i++)
            {
                if (node.choiceTexts[i] != null && !node.choiceTexts[i].IsEmpty)
                {
                    choices[i] = node.choiceTexts[i].GetLocalizedString() ?? $"Choice {i + 1}";
                }
                else
                {
                    choices[i] = $"Choice {i + 1}";
                }
            }
            
            context.onChoiceShown?.Invoke(choices);
        }
    }

    public class ActionNodeExecutor : IDialectExecutor<ActionRuntimeNode>
    {
        public void Execute(ActionRuntimeNode node, DialectExecutionContext context)
        {
            if (node.action != null)
            {
                node.action.Execute(context);
                Debug.Log($"Executed action: {node.action.name}");
            }
            else
            {
                Debug.LogWarning("Action node has no action assigned");
            }
        }
    }

    public class ConditionNodeExecutor : IDialectExecutor<ConditionRuntimeNode>
    {
        public void Execute(ConditionRuntimeNode node, DialectExecutionContext context)
        {
            if (node.condition != null)
            {
                bool result = node.condition.Evaluate(context);
                Debug.Log($"Condition {node.condition.name} evaluated to: {result}");
                context.lastConditionResult = result;
            }
            else
            {
                Debug.LogWarning("Condition node has no condition assigned");
                context.lastConditionResult = false;
            }
        }
    }

    public class DialectEndNodeExecutor : IDialectExecutor<DialectEndRuntimeNode>
    {
        public void Execute(DialectEndRuntimeNode node, DialectExecutionContext context)
        {
            Debug.Log("Ending Dialect Dialogue");
        }
    }
}