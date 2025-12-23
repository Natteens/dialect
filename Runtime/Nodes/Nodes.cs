using System;
using Dialect.Actions;
using Dialect.Conditions;
using Dialect.Core;
using Dialect.Executors;
using UnityEngine;
using UnityEngine.Localization;

namespace Dialect.Nodes
{
    [Serializable]
    public class StartRuntimeNode : RuntimeNode
    {
        public override void Execute(DialectExecutionContext context)
        {
            Debug.Log("Starting Dialect Dialogue");
        }

        public override bool ShouldAutoAdvance() => true;
    }

    [Serializable]
    public class DialogueRuntimeNode : RuntimeNode
    {
        public string speakerName;
        public string dialogueText;
        public LocalizedString _speakerLocalized;
        public LocalizedString _dialogueLocalized;

        public override void Execute(DialectExecutionContext context)
        {
            string speaker = ResolveText(speakerName, _speakerLocalized);
            string dialogue = ResolveText(dialogueText, _dialogueLocalized);
    
            context.onDialogueShown?.Invoke(speaker, dialogue);
            Debug.Log($"[{speaker}]: {dialogue}");
        }

        public override bool ShouldAutoAdvance() => false;

        string ResolveText(string fallbackText, LocalizedString localizedString)
        {
            if (localizedString != null && !localizedString.IsEmpty)
            {
                return localizedString.GetLocalizedString();
            }
            
            return fallbackText ?? string.Empty;
        }
    }
    
    [Serializable]
    public class ChoiceRuntimeNode : RuntimeNode
    {
        public string[] choiceTexts;
        public LocalizedString[] _choiceLocalized;

        public override void Execute(DialectExecutionContext context)
        {
            var choices = new string[choiceTexts.Length];
        
            for (int i = 0; i < choiceTexts.Length; i++)
            {
                if (_choiceLocalized != null && i < _choiceLocalized.Length && 
                    _choiceLocalized[i] != null && !_choiceLocalized[i].IsEmpty)
                {
                    choices[i] = _choiceLocalized[i].GetLocalizedString();
                }
                else if (choiceTexts[i] != null)
                {
                    choices[i] = choiceTexts[i];
                }
                else
                {
                    choices[i] = $"Choice {i + 1}";
                }
            }
        
            context.onChoiceShown?.Invoke(choices);
        }

        public override bool ShouldAutoAdvance() => false;
    }

    [Serializable]
    public class ActionRuntimeNode : RuntimeNode
    {
        public DialectAction action;

        public override void Execute(DialectExecutionContext context)
        {
            if (action != null)
            {
                action.Execute(context);
                Debug.Log($"Executed action: {action.name}");
            }
            else
            {
                Debug.LogWarning("Action node has no action assigned");
            }
        }

        public override bool ShouldAutoAdvance() => true;
    }

    [Serializable]
    public class ConditionRuntimeNode : RuntimeNode
    {
        public DialectCondition condition;

        public override void Execute(DialectExecutionContext context)
        {
            if (condition != null)
            {
                bool result = condition.Evaluate(context);
                Debug.Log($"Condition {condition.name} evaluated to: {result}");
                context.lastConditionResult = result;
            }
            else
            {
                Debug.LogWarning("Condition node has no condition assigned");
                context.lastConditionResult = false;
            }
        }

        public override bool ShouldAutoAdvance() => true;
    }

    [Serializable]
    public class EndRuntimeNode : RuntimeNode
    {
        public override void Execute(DialectExecutionContext context)
        {
            Debug.Log("Ending Dialect Dialogue");
        }

        public override bool ShouldAutoAdvance() => false;
    }
}