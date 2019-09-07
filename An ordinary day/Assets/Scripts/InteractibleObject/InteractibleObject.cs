using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object with possible interaction
/// </summary>
public class InteractibleObject : MonoBehaviour
{
    [SerializeField]
    private InteractibleObjectData _interactibleObjectData;
    [SerializeField]
    [Tooltip("Interaction priority level")]
    private int _priorityLevel;

    private static List<InteractibleObject> _interactibleObjects = new List<InteractibleObject>();
    public delegate void InstanciationHandler(InteractibleObject interactibleObject);
    public static event InstanciationHandler OnInteractibleObjectCreated;
    public static event InstanciationHandler OnInteractibleObjectDestroyed;
    
    private void Awake()
    {
        _interactibleObjects.Add(this);
        OnInteractibleObjectCreated?.Invoke(this);
    }


    private void OnDestroy()
    {
        _interactibleObjects.Remove(this);
        OnInteractibleObjectDestroyed?.Invoke(this);
    }


    #region Accessors

    public InteractibleObjectData GetData() => _interactibleObjectData;
    public int GetPriorityLevel() => _priorityLevel;
    public static List<InteractibleObject> GetInteractibleObjects() => _interactibleObjects;

    #endregion
}
