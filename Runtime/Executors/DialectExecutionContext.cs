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
}