using System;
using Dialect.Actions;
using Dialect.Conditions;
using Dialect.Core;
using UnityEngine.Localization;

namespace Dialect.Nodes
{
    [Serializable]
    public class DialectStartRuntimeNode : DialectRuntimeNode { }

    [Serializable]
    public class DialogueRuntimeNode : DialectRuntimeNode
    {
        public string speakerName;
        public string dialogueText;
        public LocalizedString _speakerLocalized;
        public LocalizedString _dialogueLocalized;
    }
    
    [Serializable]
    public class ChoiceRuntimeNode : DialectRuntimeNode
    {
        public string[] choiceTexts;
        public LocalizedString[] _choiceLocalized;
    }

    [Serializable]
    public class ActionRuntimeNode : DialectRuntimeNode
    {
        public DialectAction action;
    }

    [Serializable]
    public class ConditionRuntimeNode : DialectRuntimeNode
    {
        public DialectCondition condition;
    }

    [Serializable]
    public class DialectEndRuntimeNode : DialectRuntimeNode { }
}