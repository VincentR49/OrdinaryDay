using UnityEngine;

/// <summary>
/// Attach this to a game object to make it pickable.
/// </summary>
public class PickableGameItem : MonoBehaviour, I_InteractionResponse
{
    [SerializeField]
    private GameItemData _gameItem;
    [SerializeField]
    private bool _showRewardPopupOnPicking = true;


    public void OnInteraction(GameObject interactor)
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
            // TODO
            Debug.LogError("TODO: show reward popup");
        }
        Destroy(gameObject);
    }
}
