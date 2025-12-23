using System.Collections.Generic;
using UnityEngine;

namespace Dialect.Core
{
    public class DialectRuntimeGraph : ScriptableObject
    {
        [SerializeReference]
        public List<RuntimeNode> nodes = new();
    }
}