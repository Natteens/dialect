using System.Collections.Generic;
using UnityEngine;

namespace Dialect.Core
{
    public class DialectRuntimeGraph : ScriptableObject
    {
        [SerializeReference]
        public List<DialectRuntimeNode> nodes = new();
    }
}