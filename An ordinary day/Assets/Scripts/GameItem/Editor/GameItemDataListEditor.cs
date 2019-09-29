using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(GameItemDataList))]
public class GameItemDataListEditor : Editor
{
    private GameItemDataList _target;

    private void OnEnable()
    {
        _target = (GameItemDataList) target;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Refresh content"))
        {
            // load all the game items included from the folder of the target
            var currentDirectory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(_target));
            _target.LoadAllFromPath(currentDirectory);
            // in order to actually save the changes...
            EditorUtility.SetDirty(_target);
        }
    }
}
