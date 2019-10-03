using UnityEngine;

/// <summary>
/// Display dialogue for a given NPC
/// </summary>
public class NPCDialogueDisplay : MonoBehaviour
{
    [SerializeField]
    protected GameObject _container;

    [SerializeField]
    private MultiPageTextHandler _multiPageText;

    public void Show(bool show)
    {
        _container.SetActive(show);
    }
}
