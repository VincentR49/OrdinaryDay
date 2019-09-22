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
            // load all the game items included from the folder of the target
            var currentDirectory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(_target));
            _target.LoadAllFromPath(currentDirectory);
        }
    }
}
