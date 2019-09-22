using UnityEngine;

/// <summary>
/// Attach this to a go to make it persistant between scenes
/// </summary>
public class PersistantBetweenScene : MonoBehaviour
{
    private void Awake()
    {
        if (transform != transform.root)
        {
            Debug.LogError("This game object is a child of another gameObject. Persistancy is not allowed");
            return;
        }
        DontDestroyOnLoad(this);  
    }
}
