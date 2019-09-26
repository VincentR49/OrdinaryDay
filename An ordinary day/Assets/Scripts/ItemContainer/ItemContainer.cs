using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Attach this to a gameobject containing some game items, that can be picked by interacting with it
/// </summary>
public class ItemContainer : MonoBehaviour, I_InteractionResponse
{
    [SerializeField]
    private List<PickableGameItem> _items;
    [SerializeField]
    [Tooltip("Put false if a dialogue should start before being able to take the item")]
    private bool _takeItemDirectly = true;
    [SerializeField]
    private SpeakableObject _speakableObject;


    private ItemContainerDialogueData DialogueData => (ItemContainerDialogueData) _speakableObject.GetDialogueData();
    private bool IsEmpty => _items.Count == 0;
    private GameObject _lastInteractor;


    public void OnInteraction(GameObject interactor)
    {
        _lastInteractor = interactor;
        if (IsEmpty)
        {
            _speakableObject.SpeaksTo(interactor, DialogueData.EmptyNode);
        }
        else
        {
            if (_takeItemDirectly)
            {
                TakeItems(interactor);
            }
            else
            {
                _speakableObject.SpeaksTo(interactor, DialogueData.NotEmptyNode);
                _speakableObject.OnDialogueFinished += OnDialogueFinished;
            }
        }
    }


    private void OnDialogueFinished(List<string> visitedNodes)
    {
        // We take the item only if the player has choose to take it
        _speakableObject.OnDialogueFinished -= OnDialogueFinished;
        foreach (var node in visitedNodes)
        {
            if (DialogueData.JustTookItemNode.Equals(node))
            {
                TakeItems(_lastInteractor);
            }
        }
    }


    private void TakeItems(GameObject interactor)
    {
        foreach (var item in _items)
        {
            item.PickUpObject(interactor);
        }
    }
}
