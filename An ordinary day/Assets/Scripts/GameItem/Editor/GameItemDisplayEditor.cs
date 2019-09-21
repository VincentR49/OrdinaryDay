using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GameItemDisplay))]
public class GameItemDisplayEditor : Editor
{
    GameItemDisplay _target;

    void OnEnable()
    {
        _target =  (GameItemDisplay) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("RefreshDisplay"))
        {
            _target.RefreshDisplay();
        }
    }
}
