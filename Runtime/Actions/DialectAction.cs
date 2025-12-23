using Dialect.Executors;
using UnityEngine;

namespace Dialect.Actions
{
    public abstract class DialectAction : ScriptableObject
    {
        public abstract void Execute(DialectExecutionContext context);
    }
}