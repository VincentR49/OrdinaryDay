using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName ="Scriptables/GamePosition")]
public class GamePositionData : VariableData<GamePositionReference>
{
    public override string ToString()
    {
        return Value.Scene.Path + " - " + Value.Position.ToString();
    }

    public bool IsInCurrentScene() => Value.Scene.Path.Equals(SceneManager.GetActiveScene().path);
}
