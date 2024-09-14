using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Visualize.Utils;

namespace Visualize.Core;

public static partial class VisualControlTypes
{
    private static VisualControlInfo VisualClass(object target, Type type, List<VisualSpinBox> debugExportSpinBoxes, Action<object> valueChanged)
    {
        VBoxContainer vbox = new();

        if (target != null)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

            PropertyInfo[] properties = type.GetProperties(flags);

            foreach (PropertyInfo property in properties)
            {
                object initialValue = property.GetValue(target);

                MethodInfo propertySetMethod = property.GetSetMethod(true);

                VisualControlInfo control = CreateControlForType(initialValue, property.PropertyType, debugExportSpinBoxes, v =>
                {
                    property.SetValue(target, v);
                    valueChanged(target);
                });

                if (control.VisualControl != null)
                {
                    if (propertySetMethod == null)
                    {
                        control.VisualControl.SetEditable(false);
                    }

                    vbox.AddChild(CreateHBoxForMember(property.Name, control.VisualControl.Control));
                }
            }

            FieldInfo[] fields = type
                .GetFields(flags)
                .Where(f => !f.Name.StartsWith("<") || !f.Name.EndsWith(">k__BackingField"))
                .ToArray();

            foreach (FieldInfo field in fields)
            {
                object initialValue = field.GetValue(target);

                VisualControlInfo control = CreateControlForType(initialValue, field.FieldType, debugExportSpinBoxes, v =>
                {
                    field.SetValue(target, v);
                    valueChanged(target);
                });

                if (control.VisualControl != null)
                {
                    control.VisualControl.SetEditable(!field.IsLiteral);
                    vbox.AddChild(CreateHBoxForMember(field.Name, control.VisualControl.Control));
                }
            }

            // Cannot include private methods or else we will see Godots built in methods
            MethodInfo[] methods = type
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_")).ToArray();

            foreach (MethodInfo method in methods)
            {
                ParameterInfo[] paramInfos = method.GetParameters();
                object[] providedValues = new object[paramInfos.Length];

                HBoxContainer hboxParams = VisualMethods.CreateMethodParameterControls(method, debugExportSpinBoxes, providedValues);
                Button button = VisualMethods.CreateMethodButton(method, target, paramInfos, providedValues);
                
                vbox.AddChild(hboxParams);
                vbox.AddChild(button);
            }
        }

        return new VisualControlInfo(new VBoxContainerControl(vbox));
    }

    private static HBoxContainer CreateHBoxForMember(string memberName, Control control)
    {
        HBoxContainer hbox = new();
        hbox.AddChild(new Label { Text = memberName.ToPascalCase().AddSpaceBeforeEachCapital() });
        hbox.AddChild(control);
        return hbox;
    }
}
