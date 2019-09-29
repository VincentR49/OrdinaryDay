using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameItemDialogueData))]
public class GameItemDialogueDataEditor : Editor
{
    private GameItemDialogueData _target;

    void OnEnable()
    {
        _target = (GameItemDialogueData) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Refresh Content"))
        {
            _target.UpdateContent();
            // in order to actually save the changes...
            EditorUtility.SetDirty(_target);
        }
    }
}
