using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/SpawnData")]
public class SpawnData : ScriptableObject
{
    public Direction Direction;
    public GamePositionData GamePosition;
    public bool IsInCurrentScene() => GamePosition.IsInCurrentScene();
    // sugar
    public string ScenePath => GamePosition.Value.Scene.Path;
    public Vector2 Position => GamePosition.Value.Position;
}
