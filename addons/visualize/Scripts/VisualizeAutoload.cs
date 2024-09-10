using Godot;
using System;
using System.Collections.Generic;
using Visualize.Utils;

namespace Visualize.Core;

public partial class VisualizeAutoload : Node
{
    private Dictionary<ulong, List<Action>> updateControls = new();

	public override void _Ready()
	{
        foreach (Node node in GetTree().Root.GetChildren<Node>())
        {
            VisualNode visualNode = VisualizeAttributeHandler.RetrieveData(node);

            if (visualNode != null)
            {
                updateControls.Add(node.GetInstanceId(), VisualUI.CreateVisualPanel(visualNode));
            }
        }

        GetTree().NodeAdded += node =>
        {
            VisualNode visualNode = VisualizeAttributeHandler.RetrieveData(node);

            if (visualNode != null)
            {
                updateControls.Add(node.GetInstanceId(), VisualUI.CreateVisualPanel(visualNode));
            }
        };

        GetTree().NodeRemoved += node =>
        {
            updateControls.Remove(node.GetInstanceId());
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        foreach (KeyValuePair<ulong, List<Action>> kvp in updateControls)
        {
            foreach (Action action in kvp.Value)
            {
                action();
            }
        }
    }
}
