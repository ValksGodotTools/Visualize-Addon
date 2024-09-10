using Godot;
using System;
using System.Collections.Generic;

namespace Visualize;

public partial class VisualizeAutoload : Node
{
    private List<Action> updateControls;

	public override void _Ready()
	{
        List<VisualNode> visualAttributeData = VisualizeAttributeHandler.RetrieveData(GetTree().Root);

        if (visualAttributeData.Count > 0)
        {
            List<VisualSpinBox> debugExportSpinBoxes = new();

            updateControls = VisualUI.CreateVisualPanels(visualAttributeData, debugExportSpinBoxes);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        foreach (Action action in updateControls)
        {
            action();
        }
    }
}
