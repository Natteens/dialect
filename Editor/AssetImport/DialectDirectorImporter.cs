using System;
using System.Collections.Generic;
using System.Linq;
using Dialect.Actions;
using Dialect.Conditions;
using Dialect.Core;
using Dialect.Editor.Nodes;
using Dialect.Nodes;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Localization;

namespace Dialect.Editor.AssetImport
{
    [ScriptedImporter(1, DialectDirectorGraph.AssetExtension)]
    internal class DialectDirectorImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var graph = GraphDatabase.LoadGraphForImporter<DialectDirectorGraph>(ctx.assetPath);
            
            if (graph == null)
            {
                Debug.LogError($"Failed to load Dialect Director graph asset: {ctx.assetPath}");
                return;
            }
            
            var startNodeModel = graph.GetNodes().OfType<StartNode>().FirstOrDefault();
            
            if (startNodeModel == null)
            {
                Debug.LogError("No start node found in graph");
                return;
            }
            
            var runtimeAsset = ScriptableObject.CreateInstance<DialectRuntimeGraph>();
            var nodeMap = new Dictionary<INode, int>();
            
            CreateRuntimeNodes(startNodeModel, runtimeAsset, nodeMap);
            SetupConnections(startNodeModel, runtimeAsset, nodeMap);
            
            ctx.AddObjectToAsset("RuntimeAsset", runtimeAsset);
            ctx.SetMainObject(runtimeAsset);
        }

        void CreateRuntimeNodes(INode startNode, DialectRuntimeGraph runtimeGraph, Dictionary<INode, int> nodeMap)
        {
            var nodesToProcess = new Queue<INode>();
            nodesToProcess.Enqueue(startNode);

            while (nodesToProcess.Count > 0)
            {
                var currentNode = nodesToProcess.Dequeue();
                
                if (nodeMap.ContainsKey(currentNode)) continue;
                
                var runtimeNodes = TranslateNodeModelToRuntimeNodes(currentNode);

                foreach (var runtimeNode in runtimeNodes)
                {
                    nodeMap[currentNode] = runtimeGraph.nodes.Count;
                    runtimeGraph.nodes.Add(runtimeNode);
                }
                
                for (int i = 0; i < currentNode.outputPortCount; i++)
                {
                    var port = currentNode.GetOutputPort(i);

                    if (port.isConnected)
                    {
                        nodesToProcess.Enqueue(port.firstConnectedPort.GetNode());
                    }
                }
            }
        }

        void SetupConnections(INode startNode, DialectRuntimeGraph runtimeGraph, Dictionary<INode, int> nodeMap)
        {
            foreach (var kvp in nodeMap)
            {
                var editorNode = kvp.Key;
                var runtimeIndex = kvp.Value;
                var runtimeNode = runtimeGraph.nodes[runtimeIndex];

                for (int i = 0; i < editorNode.outputPortCount; i++)
                {
                    var port = editorNode.GetOutputPort(i);

                    if (port.isConnected && nodeMap.TryGetValue(port.firstConnectedPort.GetNode(), out int nextIndex))
                    {
                        runtimeNode.nextNodeIndices.Add(nextIndex);
                    }
                }
            }
        }

        static List<DialectRuntimeNode> TranslateNodeModelToRuntimeNodes(INode nodeModel)
        {
            var returnedNodes = new List<DialectRuntimeNode>();

            switch (nodeModel)
            {
                case StartNode:
                    returnedNodes.Add(new DialectStartRuntimeNode());
                    break;
                    
                case DialogueNode dialogueNode:
                    var speakerPort = dialogueNode.GetInputPortByName("SpeakerName");
                    var dialoguePort = dialogueNode.GetInputPortByName("DialogueText");
                    
                    LocalizedString speakerName = GetInputPortValue<LocalizedString>(speakerPort);
                    LocalizedString dialogueText = GetInputPortValue<LocalizedString>(dialoguePort);
                    
                    returnedNodes.Add(new DialogueRuntimeNode
                    {
                        speakerName = speakerName,
                        dialogueText = dialogueText
                    });
                    break;
                    
                case ChoiceNode choiceNode:
                    var choiceTexts = new List<LocalizedString>();
                    int portCount = 2;
                    choiceNode.GetNodeOptionByName("portCount")?.TryGetValue(out portCount);
                    
                    for (int i = 0; i < portCount; i++)
                    {
                        var choicePort = choiceNode.GetInputPortByName($"ChoiceText{i}");
                        var choiceText = GetInputPortValue<LocalizedString>(choicePort);
                        choiceTexts.Add(choiceText);
                    }
                    
                    returnedNodes.Add(new ChoiceRuntimeNode
                    {
                        choiceTexts = choiceTexts.ToArray()
                    });
                    break;
                    
                case ActionNode actionNode:
                    var actionPort = actionNode.GetInputPortByName("action");
                    var action = GetInputPortValue<DialectAction>(actionPort);
                    
                    returnedNodes.Add(new ActionRuntimeNode
                    {
                        action = action
                    });
                    break;
                    
                case ConditionNode conditionNode:
                    var conditionPort = conditionNode.GetInputPortByName("condition");
                    var condition = GetInputPortValue<DialectCondition>(conditionPort);
                    
                    returnedNodes.Add(new ConditionRuntimeNode
                    {
                        condition = condition
                    });
                    break;
                    
                case EndNode:
                    returnedNodes.Add(new DialectEndRuntimeNode());
                    break;
                    
                default:
                    throw new ArgumentException($"Unsupported node type: {nodeModel.GetType()}");
            }
            
            return returnedNodes;
        }

        static T GetInputPortValue<T>(IPort port)
        {
            T value = default;

            if (port != null && port.isConnected)
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
            else if (port != null)
            {
                port.TryGetValue(out value);
            }
            
            return value;
        }
    }
}