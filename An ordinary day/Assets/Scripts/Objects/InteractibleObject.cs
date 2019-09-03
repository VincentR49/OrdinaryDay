using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object with possible interaction
/// </summary>
public class InteractibleObject : MonoBehaviour
{
    [SerializeField]
    private InteractibleObjectData _interactibleObjectData;

    private static List<InteractibleObject> _interactibleObjects = new List<InteractibleObject>();

    
    private void Awake()
    {
        _interactibleObjects.Add(this);
    }


    private void OnDestroy()
    {
        _interactibleObjects.Remove(this);
    }


    #region Accessors

    public InteractibleObjectData GetData() => _interactibleObjectData;

    public static List<InteractibleObject> GetInteractibleObjects() => _interactibleObjects;

    #endregion
}
