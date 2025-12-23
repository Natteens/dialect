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
        
            if (node._speakerLocalized != null && !node._speakerLocalized.IsEmpty)
            {
                speaker = node._speakerLocalized.GetLocalizedString();
            }
            else if (!string.IsNullOrEmpty(node.speakerName))
            {
                speaker = node.speakerName;
            }
        
            if (node._dialogueLocalized != null && !node._dialogueLocalized.IsEmpty)
            {
                dialogue = node._dialogueLocalized.GetLocalizedString();
            }
            else if (!string.IsNullOrEmpty(node.dialogueText))
            {
                dialogue = node.dialogueText;
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
                if (node._choiceLocalized != null && i < node._choiceLocalized.Length && 
                    node._choiceLocalized[i] != null && !node._choiceLocalized[i].IsEmpty)
                {
                    choices[i] = node._choiceLocalized[i].GetLocalizedString();
                }
                else if (node.choiceTexts[i] != null)
                {
                    choices[i] = node.choiceTexts[i];
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