using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object with possible interaction
/// Basically a holder for InteractibleObjectData
/// </summary>
public class InteractibleObject : MonoBehaviour
{
    [SerializeField]
    private InteractibleObjectData _data;
    [SerializeField]
    [Tooltip("Interaction priority level")]
    private int _priorityLevel;

    public delegate void InstanciationHandler(InteractibleObjectData data);
    public static event InstanciationHandler OnInteractibleObjectCreated;
    public static event InstanciationHandler OnInteractibleObjectDestroyed;
    private PlayerDialogueRunner _dialogueRunner;


    private void Start()
    {
        OnInteractibleObjectCreated?.Invoke(_data);
        _dialogueRunner = FindObjectOfType<PlayerDialogueRunner>(); // change this later
    }


    private void OnDestroy()
    {
        OnInteractibleObjectDestroyed?.Invoke(_data);
    }


    public void InteractWith(GameObject interactor)
    {
        // TODO implement logic here
        _dialogueRunner.StartDialogue(_data.DefaultNodeStory);
    }

    #region Accessors

    public InteractibleObjectData GetData() => _data;
    public int GetPriorityLevel() => _priorityLevel;

    #endregion
}
