using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualize.Core;

public static partial class VisualControlTypes
{
    private static VisualControlInfo Object(object initialValue, Action<object> valueChanged)
    {
        LineEdit lineEdit = new() { Text = initialValue?.ToString() ?? string.Empty };
        lineEdit.TextChanged += text => valueChanged(text);

        return new VisualControlInfo(new LineEditControl(lineEdit));
    }
}
