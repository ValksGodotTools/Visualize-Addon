using Godot;
using System.Collections.Generic;

namespace Visualize;

public partial class VisualizeAutoload : Node
{
    private List<VisualNode> _alwaysUpdateNodes;

	public override void _Ready()
	{
        List<VisualNode> visualAttributeData = VisualizeAttributeHandler.RetrieveData(GetTree().Root);

        if (visualAttributeData.Count > 0)
        {
            List<VisualSpinBox> debugExportSpinBoxes = new();

            VisualUI.CreateVisualPanels(visualAttributeData, debugExportSpinBoxes);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        
    }
}
