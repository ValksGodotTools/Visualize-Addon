using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visualize.Utils;

namespace Visualize.Core;

public static partial class VisualControlTypes
{
    private static VisualControlInfo Color(object initialValue, Action<Color> valueChanged)
    {
        Color initialColor = (Color)initialValue;

        GColorPickerButton colorPickerButton = new(initialColor);
        colorPickerButton.OnColorChanged += color => valueChanged(color);

        return new VisualControlInfo(new ColorPickerButtonControl(colorPickerButton));
    }
}

public class ColorPickerButtonControl : IVisualControl
{
    private readonly GColorPickerButton _colorPickerButton;

    public ColorPickerButtonControl(GColorPickerButton colorPickerButton)
    {
        _colorPickerButton = colorPickerButton;
    }

    public void SetValue(object value)
    {
        if (value is Color color)
        {
            _colorPickerButton.Control.Color = color;
        }
    }

    public Control Control => _colorPickerButton.Control;

    public void SetEditable(bool editable)
    {
        _colorPickerButton.Control.Disabled = !editable;
    }
}
