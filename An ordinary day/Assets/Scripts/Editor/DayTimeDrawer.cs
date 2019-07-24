using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DayTime))]
public class DayTimeDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var hourRect = new Rect(position.x, position.y, 30, position.height);
        var minRect = new Rect(position.x + 40, position.y, 30, position.height);
        var secRect = new Rect(position.x + 80, position.y, 30, position.height);
       
        // Labels rect
        var dateSeparatorRect1 = new Rect(minRect.x - 10, position.y, 10, position.height);
        var dateSeparatorRect2 = new Rect(secRect.x - 10, position.y, 10, position.height);
       
        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(hourRect, property.FindPropertyRelative("Hour"), GUIContent.none);
        EditorGUI.LabelField(dateSeparatorRect1, ":");
        EditorGUI.PropertyField(minRect, property.FindPropertyRelative("Min"), GUIContent.none);
        EditorGUI.LabelField(dateSeparatorRect2, ":");
        EditorGUI.PropertyField(secRect, property.FindPropertyRelative("Sec"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
