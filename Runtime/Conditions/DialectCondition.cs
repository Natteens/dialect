using Dialect.Executors;
using UnityEngine;

namespace Dialect.Conditions
{
    public abstract class DialectCondition : ScriptableObject
    {
        public abstract bool Evaluate(DialectExecutionContext context);
    }
}