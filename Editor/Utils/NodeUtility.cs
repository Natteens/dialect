using Unity.GraphToolkit.Editor;

namespace Dialect.Editor.Utils
{
    /// <summary>
    /// Utility methods for editor nodes
    /// </summary>
    internal static class NodeUtility
    {
        /// <summary>
        /// Gets the value from an input port, checking connected nodes or the port's own value
        /// </summary>
        public static T GetInputPortValue<T>(IPort port)
        {
            T value = default;

            if (port == null) return value;

            if (port.IsConnected)
            {
                switch (port.FirstConnectedPort.GetNode())
                {
                    case IVariableNode variableNode:
                        variableNode.Variable.TryGetDefaultValue(out value);
                        break;
                    case IConstantNode constantNode:
                        constantNode.TryGetValue<T>(out value);
                        break;
                }
            }
            else
            {
                port.TryGetValue(out value);
            }
            
            return value;
        }
    }
}