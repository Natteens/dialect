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
        public LocalizedString speakerName;
        public LocalizedString dialogueText;
    }

    [Serializable]
    public class ChoiceRuntimeNode : DialectRuntimeNode
    {
        public LocalizedString[] choiceTexts;
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