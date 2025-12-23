# Dialect

![Package: dialect](https://img.shields.io/badge/Package-dialect-blue.svg)
![Platform: Unity](https://img.shields.io/badge/Platform-Unity-7f7f7f.svg)
![Localization: Supported](https://img.shields.io/badge/Localization-Supported-green.svg)
![GraphToolkit: Integrated](https://img.shields.io/badge/GraphToolkit-Integrated-orange.svg) 
![Status: Experimental](https://img.shields.io/badge/Status-Experimental-yellow.svg)
![Version: 0.1.2](https://img.shields.io/badge/Version-0.1.2-lightgrey.svg)

A modular, graph-based dialogue system for Unity with first-class localization support and editor tooling integration (Graph Tools).

Table of Contents
- Overview
- Key Concepts
- Nodes
- Localization
- Runtime API
- Example Usage
- Extensibility
- How to create a graph in the Editor
- Best Practices
- Contributing & Next Steps

## Overview

Dialect allows you to author conversations as visual graphs in the Unity Editor and converts them into a compact runtime representation (`DialectRuntimeGraph`). At runtime a `DialectDirector` component executes the graph, raising events for UI and controlling flow, choices and conditional branching.

Main features:
- Visual authoring using Graph Toolkit
- Importer that converts editor graph to `DialectRuntimeGraph` (ScriptableObject)
- Localization-ready (uses Unity's `LocalizedString` where needed)
- Built-in nodes: Dialogue, Choice, Condition, Action, End, and a reusable Localized node

## Key Concepts

- Graph (`DialectDirectorGraph`): the editable asset you create in the Editor.
- Editor Nodes: visual building blocks used while creating dialogues.
- Runtime Nodes (`RuntimeNode`): serializable nodes inside `DialectRuntimeGraph` used by the director at runtime.
- `DialectDirector`: MonoBehaviour that runs the runtime graph and exposes events for UI.

## Nodes (summary)

- StartNode — entry point of the graph
- DialogueNode — holds speaker and text (string or `LocalizedString`)
- ChoiceNode — multiple choice options (strings or `LocalizedString` references)
- ConditionNode — evaluates a `DialectCondition` and branches to True/False
- ActionNode — runs a `DialectAction` (custom logic)
- EndNode — ends the dialogue
- LocalizedNode — small utility node that encapsulates a `LocalizedString` for reuse

Editor nodes implement `IConvertibleToRuntime` to produce `RuntimeNode` instances during import.

## Localization

Dialect integrates with Unity's Localization package. Editor fields for speaker or text can connect to a `LocalizedNode` which stores a `LocalizedString`. When a runtime node has a `LocalizedString`, the `DialectDirector` will emit events with the localized text (and will re-process the current node when the selected locale changes via `LocalizationSettings.SelectedLocaleChanged`).

Tips:
- Use `LocalizedString` for any text that needs runtime language switching.
- Static labels can remain plain strings.

## Runtime API

Main component: `DialectDirector` (MonoBehaviour)

Public events:
- `event Action<string, string> OnDialogueShown` — invoked when a dialogue node is shown (speakerName, dialogueText).
- `event Action<string[]> OnChoiceShown` — invoked when a choice node is shown (array of choice texts).
- `event Action OnDialogueEnded` — invoked when the dialogue finishes.

Key methods:
- `void StartDialogue()` — starts the graph from the initial node assigned in `runtimeGraph`.
- `void AdvanceDialogue(int choiceIndex = 0)` — advance to the next node; specify choiceIndex when selecting choices.
- `int GetCurrentChoiceCount()` — returns number of choices in the current node.
- `bool IsRunning()` — returns whether the director is running.

Behavior notes:
- Director calls `RuntimeNode.Execute(context)` for the current node. Nodes request UI updates via the provided context callbacks.
- On locale change, the director re-processes the current node to refresh localized texts.

## Example Usage (C#)

This minimal example shows how to subscribe to the director and present text/choices in your UI layer.

```csharp
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public Dialect.DialectDirector director; // assign in inspector

    void OnEnable()
    {
        if (director == null) return;
        director.OnDialogueShown += HandleDialogue;
        director.OnChoiceShown += HandleChoices;
        director.OnDialogueEnded += HandleEnd;
    }

    void OnDisable()
    {
        if (director == null) return;
        director.OnDialogueShown -= HandleDialogue;
        director.OnChoiceShown -= HandleChoices;
        director.OnDialogueEnded -= HandleEnd;
    }

    void HandleDialogue(string speaker, string text)
    {
        // Update your UI: speaker label and body text.
    }

    void HandleChoices(string[] choices)
    {
        // Create buttons for each choice and call director.AdvanceDialogue(index) on click.
    }

    void HandleEnd()
    {
        // Hide/clear dialogue UI.
    }
}
```

## Extensibility

- Conditions and actions use interfaces (`DialectCondition`, `DialectAction`) — implement those interfaces to plug game logic into nodes.
- Create new runtime node types by subclassing `RuntimeNode` and implement a matching editor node that converts to it (implement `IConvertibleToRuntime`).

## How to create a graph in the Editor

1. Assets > Create > Dialect Director Graph
2. Open the graph with the Graph Toolkit editor, add nodes and connect execution ports and inputs
3. Save the asset — the `DialectDirectorImporter` will produce a `DialectRuntimeGraph` inside the asset which you can assign to a `DialectDirector` at runtime

## Best practices

- Keep graphs modular: use action nodes to call game logic instead of embedding it inside nodes.
- Prefer `LocalizedString` for any user-facing text that requires translation.
- Validate conditions and actions in small unit tests where possible.

## Contributing & Next Steps

If you'd like, I can also:
- Add an example scene and a simple Dialogue UI prefab that demonstrates `DialectDirector` in action.
- Provide a small sample `DialectCondition` and `DialectAction` implementation and unit tests.
- Create an English-to-Portuguese bilingual README or improved API docs.

Tags: `dialogue`, `localization`, `graph`, `unity`, `editor-tools`, `runtime`

---

If you want, tell me which of the suggested next steps to implement and I will add it to the package (example scene, prefab, tests, or extra docs).
