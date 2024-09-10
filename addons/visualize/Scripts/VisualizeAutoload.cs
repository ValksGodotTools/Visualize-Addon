using Godot;
using System;
using System.Collections.Generic;
using Visualize.Utils;

namespace Visualize.Core;

public partial class VisualizeAutoload : Node
{
    private readonly Dictionary<ulong, VisualNodeInfo> nodeTrackers = new();

	public override void _Ready()
	{
        foreach (Node node in GetTree().Root.GetChildren<Node>())
        {
            VisualNode visualNode = VisualizeAttributeHandler.RetrieveData(node);

            if (visualNode != null)
            {
                (VBoxContainer vbox, List<Action> actions) = VisualUI.CreateVisualPanel(GetTree(), visualNode);

                nodeTrackers.Add(node.GetInstanceId(), new VisualNodeInfo(actions, vbox, node));
            }
        }

        GetTree().NodeAdded += node =>
        {
            VisualNode visualNode = VisualizeAttributeHandler.RetrieveData(node);

            if (visualNode != null)
            {
                (VBoxContainer vbox, List<Action> actions) = VisualUI.CreateVisualPanel(GetTree(), visualNode);

                nodeTrackers.Add(node.GetInstanceId(), new VisualNodeInfo(actions, vbox, node));
            }
        };

        GetTree().NodeRemoved += node =>
        {
            if (nodeTrackers.ContainsKey(node.GetInstanceId()))
            {
                nodeTrackers[node.GetInstanceId()].VisualControl.QueueFree();
                nodeTrackers.Remove(node.GetInstanceId());
            }
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        foreach (KeyValuePair<ulong, VisualNodeInfo> kvp in nodeTrackers)
        {
            Node node = kvp.Value.Node;

            if (node is Node2D node2D)
            {
                kvp.Value.VisualControl.GlobalPosition = node2D.GlobalPosition;
            }
            else if (node is Control control)
            {
                kvp.Value.VisualControl.GlobalPosition = control.GlobalPosition;
            }

            foreach (Action action in kvp.Value.Actions)
            {
                action();
            }
        }
    }
}

public class VisualNodeInfo
{
    public List<Action> Actions { get; }
    public VBoxContainer VisualControl { get; }
    public Node Node { get; }

    public VisualNodeInfo(List<Action> actions, VBoxContainer visualControl, Node node)
    {
        Actions = actions ?? throw new ArgumentNullException(nameof(actions));
        VisualControl = visualControl ?? throw new ArgumentNullException(nameof(visualControl));
        Node = node ?? throw new ArgumentNullException(nameof(node));
    }
}
