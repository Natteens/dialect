using System;
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
        
        RuntimeNode currentNode;
        bool isRunning;
        public object customData;
        
        bool isListeningToLocaleChanges;

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

            var context = new DialectExecutionContext
            {
                director = this,
                onDialogueShown = OnDialogueShown,
                onChoiceShown = OnChoiceShown,
                onDialogueEnded = OnDialogueEnded,
                customData = customData
            };

            currentNode.Execute(context);

            if (currentNode is EndRuntimeNode)
            {
                EndDialogue();
            }
            else if (currentNode is ConditionRuntimeNode)
            {
                int pathIndex = context.lastConditionResult ? 0 : 1;
                AdvanceDialogue(pathIndex);
            }
            else if (currentNode.ShouldAutoAdvance())
            {
                AdvanceDialogue();
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