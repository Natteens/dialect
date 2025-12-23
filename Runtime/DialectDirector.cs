using System;
using System.Collections.Generic;
using Dialect.Core;
using Dialect.Executors;
using Dialect.Nodes;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Dialect
{
    public class DialectDirector : MonoBehaviour
    {
        [Header("Graph")]
        public DialectRuntimeGraph runtimeGraph;
        
        public event Action<string, string> OnDialogueShown;
        public event Action<string[]> OnChoiceShown;
        public event Action OnDialogueEnded;
        
        Dictionary<Type, object> executors;
        DialectRuntimeNode currentNode;
        bool isRunning;
        public object customData;
        
        bool isListeningToLocaleChanges;

        void Awake()
        {
            executors = new Dictionary<Type, object>
            {
                { typeof(DialectStartRuntimeNode), new DialectStartNodeExecutor() },
                { typeof(DialogueRuntimeNode), new DialogueNodeExecutor() },
                { typeof(ChoiceRuntimeNode), new ChoiceNodeExecutor() },
                { typeof(ActionRuntimeNode), new ActionNodeExecutor() },
                { typeof(ConditionRuntimeNode), new ConditionNodeExecutor() },
                { typeof(DialectEndRuntimeNode), new DialectEndNodeExecutor() }
            };
        }

        void OnEnable()
        {
            if (!isListeningToLocaleChanges)
            {
                LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
                isListeningToLocaleChanges = true;
            }
        }

        void OnDisable()
        {
            if (isListeningToLocaleChanges)
            {
                LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
                isListeningToLocaleChanges = false;
            }
        }

        void OnLocaleChanged(UnityEngine.Localization.Locale locale)
        {
            if (isRunning && currentNode != null)
            {
                ProcessCurrentNode();
            }
        }

        public void StartDialogue()
        {
            if (runtimeGraph == null)
            {
                Debug.LogError("No runtime graph assigned!");
                return;
            }
            
            if (runtimeGraph.nodes.Count == 0)
            {
                Debug.LogError("Runtime graph has no nodes!");
                return;
            }
            
            currentNode = runtimeGraph.nodes[0];
            isRunning = true;
            ProcessCurrentNode();
        }

        public void AdvanceDialogue(int choiceIndex = 0)
        {
            if (!isRunning || currentNode == null) return;

            if (choiceIndex >= 0 && choiceIndex < currentNode.nextNodeIndices.Count)
            {
                int nextIndex = currentNode.nextNodeIndices[choiceIndex];
                
                if (nextIndex >= 0 && nextIndex < runtimeGraph.nodes.Count)
                {
                    currentNode = runtimeGraph.nodes[nextIndex];
                    ProcessCurrentNode();
                }
                else
                {
                    EndDialogue();
                }
            }
            else
            {
                EndDialogue();
            }
        }

        void ProcessCurrentNode()
        {
            if (currentNode == null)
            {
                EndDialogue();
                return;
            }

            if (!executors.TryGetValue(currentNode.GetType(), out var executor))
            {
                Debug.LogError($"No executor found for node type: {currentNode.GetType()}");
                EndDialogue();
                return;
            }

            var context = new DialectExecutionContext
            {
                director = this,
                onDialogueShown = OnDialogueShown,
                onChoiceShown = OnChoiceShown,
                onDialogueEnded = OnDialogueEnded,
                customData = customData
            };

            if (currentNode is DialectStartRuntimeNode startNode)
            {
                ((IDialectExecutor<DialectStartRuntimeNode>)executor).Execute(startNode, context);
                AdvanceDialogue(0);
            }
            else if (currentNode is DialogueRuntimeNode dialogueNode)
            {
                ((IDialectExecutor<DialogueRuntimeNode>)executor).Execute(dialogueNode, context);
            }
            else if (currentNode is ChoiceRuntimeNode choiceNode)
            {
                ((IDialectExecutor<ChoiceRuntimeNode>)executor).Execute(choiceNode, context);
            }
            else if (currentNode is ActionRuntimeNode actionNode)
            {
                ((IDialectExecutor<ActionRuntimeNode>)executor).Execute(actionNode, context);
                AdvanceDialogue(0);
            }
            else if (currentNode is ConditionRuntimeNode conditionNode)
            {
                ((IDialectExecutor<ConditionRuntimeNode>)executor).Execute(conditionNode, context);
                int pathIndex = context.lastConditionResult ? 0 : 1;
                AdvanceDialogue(pathIndex);
            }
            else if (currentNode is DialectEndRuntimeNode endNode)
            {
                ((IDialectExecutor<DialectEndRuntimeNode>)executor).Execute(endNode, context);
                EndDialogue();
            }
        }

        void EndDialogue()
        {
            isRunning = false;
            currentNode = null;
            OnDialogueEnded?.Invoke();
        }

        public int GetCurrentChoiceCount()
        {
            return currentNode?.nextNodeIndices.Count ?? 0;
        }

        public bool IsRunning()
        {
            return isRunning;
        }
    }
}