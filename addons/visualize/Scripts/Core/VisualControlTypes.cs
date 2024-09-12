using Godot;
using System;
using System.Collections.Generic;
using Visualize.Utils;

namespace Visualize.Core;

public static partial class VisualControlTypes
{
    public static VisualControlInfo CreateControlForType(object initialValue, Type type, List<VisualSpinBox> debugExportSpinBoxes, Action<object> valueChanged)
    {
        VisualControlInfo info = type switch
        {
            _ when type == typeof(bool) => Bool(initialValue, v => valueChanged(v)),
            _ when type == typeof(string) => String(initialValue, v => valueChanged(v)),
            _ when type == typeof(object) => Object(initialValue, v => valueChanged(v)),
            _ when type == typeof(Color) => Color(initialValue, v => valueChanged(v)),
            _ when type == typeof(Vector2) => Vector2(initialValue, v => valueChanged(v)),
            _ when type == typeof(Vector2I) => Vector2I(initialValue, v => valueChanged(v)),
            _ when type == typeof(Vector3) => Vector3(initialValue, v => valueChanged(v)),
            _ when type == typeof(Vector3I) => Vector3I(initialValue, v => valueChanged(v)),
            _ when type == typeof(Vector4) => Vector4(initialValue, v => valueChanged(v)),
            _ when type == typeof(Vector4I) => Vector4I(initialValue, v => valueChanged(v)),
            _ when type == typeof(Quaternion) => Quaternion(initialValue, v => valueChanged(v)),
            _ when type == typeof(NodePath) => NodePath(initialValue, v => valueChanged(v)),
            _ when type == typeof(StringName) => StringName(initialValue, v => valueChanged(v)),
            _ when type.IsNumericType() => Numeric(initialValue, type, debugExportSpinBoxes, v => valueChanged(v)),
            _ when type.IsEnum => Enum(initialValue, type, v => valueChanged(v)),
            _ when type.IsArray => Array(initialValue, type, debugExportSpinBoxes, v => valueChanged(v)),
            _ when type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) => List(initialValue, type, debugExportSpinBoxes, v => valueChanged(v)),
            _ when type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>) => Dictionary(initialValue, type, debugExportSpinBoxes, v => valueChanged(v)),
            _ when type.IsClass && !type.IsSubclassOf(typeof(GodotObject)) => Class(initialValue, type, debugExportSpinBoxes, v => valueChanged(v)),
            _ when type.IsValueType && !type.IsClass && !type.IsSubclassOf(typeof(GodotObject)) => Class(initialValue, type, debugExportSpinBoxes, v => valueChanged(v)),
            _ => new VisualControlInfo(null)
        };

        if (info.VisualControl == null)
        {
            GD.PushWarning($"The type '{type}' is not supported for the {nameof(VisualizeAttribute)}");
        }

        return info;
    }
}

public interface IVisualControl
{
    void SetValue(object value);
    Control Control { get; }
    void SetEditable(bool editable);
}

public class VisualControlInfo
{
    public IVisualControl VisualControl { get; }

    public VisualControlInfo(IVisualControl visualControl)
    {
        VisualControl = visualControl;
    }
}
