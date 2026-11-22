using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// 1. Define the attribute to use in your gameplay scripts
public class InspectorLabelAttribute : PropertyAttribute
{
    public string label;
    public InspectorLabelAttribute(string label) { this.label = label; }
}

// 2. Define the drawer to change the text in the Inspector
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(InspectorLabelAttribute))]
public class InspectorLabelDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        InspectorLabelAttribute labelAttr = (InspectorLabelAttribute)attribute;
        EditorGUI.PropertyField(position, property, new GUIContent(labelAttr.label));
    }
}
#endif
