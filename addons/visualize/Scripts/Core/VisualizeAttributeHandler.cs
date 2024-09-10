using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Visualize.Utils;

namespace Visualize;

public static class VisualizeAttributeHandler
{
    private static readonly BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

    public static List<VisualNode> RetrieveData(Node parent)
    {
        List<VisualNode> debugVisualNodes = new();
        Type[] types = Assembly.GetExecutingAssembly().GetTypes();

        foreach (Type type in types)
        {
            VisualizeAttribute attribute = (VisualizeAttribute)type.GetCustomAttribute(typeof(VisualizeAttribute), false);

            Vector2 initialPosition = Vector2.Zero;
            bool alwaysUpdate = false;
            
            if (attribute != null)
            {
                initialPosition = attribute.InitialPosition;
                alwaysUpdate = attribute.AlwaysUpdate;
            }

            List<Node> nodes = parent.GetNodes(type);

            foreach (Node node in nodes)
            {
                List<PropertyInfo> properties = GetVisualMembers(type.GetProperties);
                List<FieldInfo> fields = GetVisualMembers(type.GetFields);
                List<MethodInfo> methods = GetVisualMembers(type.GetMethods);

                if (properties.Any() || fields.Any() || methods.Any())
                {
                    debugVisualNodes.Add(new VisualNode(node, initialPosition, alwaysUpdate, properties, fields, methods));
                }
            }
        }

        return debugVisualNodes;
    }

    private static List<T> GetVisualMembers<T>(Func<BindingFlags, T[]> getMembers) where T : MemberInfo
    {
        return getMembers(Flags)
            .Where(member => member.GetCustomAttributes(typeof(VisualizeAttribute), false).Any())
            .ToList();
    }
}
