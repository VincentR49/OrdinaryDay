using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(DialogueAgentDataList))]
public class DialogueAgentDataListEditor : Editor
{
    private DialogueAgentDataList _target;

    private void OnEnable()
    {
        _target = (DialogueAgentDataList)target;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Refresh content from directory"))
        {
            serializedObject.Update();
            var currentDirectory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(_target));
            _target.LoadAllFromPath(currentDirectory);
            // in order to actually save the changes...
            EditorUtility.SetDirty(_target);
        }
    }
}
