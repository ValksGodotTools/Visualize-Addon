using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Visualize;

public class VisualNode
{
    public Node Node { get; }
    public bool AlwaysUpdate { get; }
    public Vector2 InitialPosition { get; }
    public IEnumerable<PropertyInfo> Properties { get; }
    public IEnumerable<FieldInfo> Fields { get; }
    public IEnumerable<MethodInfo> Methods { get; }

    public VisualNode(Node node, Vector2 initialPosition, bool alwaysUpdate, IEnumerable<PropertyInfo> properties, IEnumerable<FieldInfo> fields, IEnumerable<MethodInfo> methods)
    {
        Node = node;
        AlwaysUpdate = alwaysUpdate;
        InitialPosition = initialPosition;
        Properties = properties;
        Fields = fields;
        Methods = methods;
    }
}

public class VisualSpinBox
{
    public SpinBox SpinBox { get; set; }
    public Type Type { get; set; }
}
