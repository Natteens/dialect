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

            if (port.isConnected)
            {
                switch (port.firstConnectedPort.GetNode())
                {
                    case IVariableNode variableNode:
                        variableNode.variable.TryGetDefaultValue<T>(out value);
                        return value;
                    case IConstantNode constantNode:
                        constantNode.TryGetValue<T>(out value);
                        return value;
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