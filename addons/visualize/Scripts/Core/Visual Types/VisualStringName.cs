using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualize.Core;

public static partial class VisualControlTypes
{
    private static VisualControlInfo StringName(object initialValue, Action<StringName> valueChanged)
    {
        StringName stringName = (StringName)initialValue;
        string initialText = stringName != null ? stringName.ToString() : string.Empty;

        LineEdit lineEdit = new() { Text = initialText };
        lineEdit.TextChanged += text => valueChanged(new StringName(text));

        return new VisualControlInfo(new LineEditControl(lineEdit));
    }
}
