using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Attach this to a gameobject containing some game items, that can be picked by interacting with it
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ItemContainer : MonoBehaviour, I_InteractionResponse
{
    [SerializeField]
    private List<PickableGameItem> _items;
    [SerializeField]
    [Tooltip("Put false if a dialogue should start before being able to take the item")]
    private bool _takeItemDirectly = true;
    [SerializeField]
    private DialogueWithPlayerAgent _dialogueWithPlayerAgent;

    [Header("Optional")]
    [SerializeField]
    private Sprite _openedSprite;

    private SpriteRenderer _spriteRenderer;
    private ItemContainerDialogueData DialogueData => (ItemContainerDialogueData) _dialogueWithPlayerAgent.GetDialogueData();
    private bool IsEmpty => _items == null || _items.Count == 0;
    private GameObject _lastInteractor;
    private bool _opened;
    private Sprite _defaultSprite;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSprite = _spriteRenderer.sprite;
    }


    public void OnInteraction(GameObject interactor)
    {
        _lastInteractor = interactor;
        if (IsEmpty)
        {
            _dialogueWithPlayerAgent.SpeaksTo(interactor, DialogueData.EmptyNode);
        }
        else
        {
            if (_takeItemDirectly)
            {
                TakeItems(interactor);
            }
            else
            {
                _dialogueWithPlayerAgent.SpeaksTo(interactor, DialogueData.NotEmptyNode);
                _dialogueWithPlayerAgent.OnDialogueFinished += OnDialogueFinished;
            }
        }
    }


    private void OnDialogueFinished(List<string> visitedNodes)
    {
        // We take the item only if the player has choose to take it
        _dialogueWithPlayerAgent.OnDialogueFinished -= OnDialogueFinished;
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
        _items.Clear();
        _opened = true;
        RefreshSprite();
    }


    private void RefreshSprite()
    {
        if (_openedSprite == null)
            return;
        _spriteRenderer.sprite = _opened ? _openedSprite : _defaultSprite;
    }
}
