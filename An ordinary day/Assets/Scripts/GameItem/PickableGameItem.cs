using UnityEngine;

/// <summary>
/// Attach this to a game object to make it pickable.
/// </summary>
public class PickableGameItem : MonoBehaviour, I_InteractionResponse
{
    [SerializeField]
    private GameItemData _gameItem;
    [SerializeField]
    private SpeakableObject _speakableObject;
    [SerializeField]
    private bool _showRewardPopupOnPicking = true;


    private GameItemDialogueData GameItemDialogueData => (GameItemDialogueData) _speakableObject.GetDialogueData();

    public void OnInteraction(GameObject interactor)
    {
        PickUpObject(interactor);
    }


    public void PickUpObject(GameObject interactor)
    {
        var inventoryHolder = interactor.GetComponent<InventoryHolder>();
        if (inventoryHolder == null)
        {
            Debug.Log("interactor has no inventory. Cannot pick up the item.");
            return;
        }
        inventoryHolder.AddItem(_gameItem.Tag);
        if (_showRewardPopupOnPicking)
        {
            ShowRewardDialogue(interactor);
        }
        Destroy(gameObject);
    }



    private void ShowRewardDialogue(GameObject interactor)
    {
        _speakableObject.SpeaksTo(interactor, GameItemDialogueData.RewardNode,
                                    yarnFile:GameItemDialogueData.RewardDialogueFile);
    }
}
