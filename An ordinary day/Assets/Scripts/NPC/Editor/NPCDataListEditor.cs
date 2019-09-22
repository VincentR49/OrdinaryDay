using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(NPCDataList))]
public class NPCDataListEditor : Editor
{
    private NPCDataList _target;

    private void OnEnable()
    {
        _target = (NPCDataList)target;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Refresh content"))
        {
            // load all the game items included from the folder of the target
            var currentDirectory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(_target));
            _target.LoadAllFromPath(currentDirectory);
        }
    }
}

