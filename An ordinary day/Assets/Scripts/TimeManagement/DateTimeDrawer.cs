using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SerialDateTime))]
public class DateTimeDrawer : PropertyDrawer
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
        var yearRect = new Rect(position.x, position.y, 40, position.height);
        var monthRect = new Rect(position.x + 50, position.y, 30, position.height);
        var dayRect = new Rect(position.x + 90, position.y, 30, position.height);

        var hourRect = new Rect(position.x + 140, position.y, 30, position.height);
        var minRect = new Rect(position.x + 180, position.y, 30, position.height);
        var secRect = new Rect(position.x + 220, position.y, 30, position.height);

        // Labels rect
        var dateSeparatorRect1 = new Rect(monthRect.x - 10, position.y, 10, position.height);
        var dateSeparatorRect2 = new Rect(dayRect.x - 10, position.y, 10, position.height);
        var dateSeparatorRect3 = new Rect(minRect.x - 10, position.y, 10, position.height);
        var dateSeparatorRect4 = new Rect(secRect.x - 10, position.y, 10, position.height);


        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(yearRect, property.FindPropertyRelative("Year"), GUIContent.none);
        EditorGUI.LabelField(dateSeparatorRect1, "/");
        EditorGUI.PropertyField(monthRect, property.FindPropertyRelative("Month"), GUIContent.none);
        EditorGUI.LabelField(dateSeparatorRect2, "/");
        EditorGUI.PropertyField(dayRect, property.FindPropertyRelative("Day"), GUIContent.none);

        EditorGUI.PropertyField(hourRect, property.FindPropertyRelative("Hour"), GUIContent.none);
        EditorGUI.LabelField(dateSeparatorRect3, ":");
        EditorGUI.PropertyField(minRect, property.FindPropertyRelative("Min"), GUIContent.none);
        EditorGUI.LabelField(dateSeparatorRect4, ":");
        EditorGUI.PropertyField(secRect, property.FindPropertyRelative("Sec"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
