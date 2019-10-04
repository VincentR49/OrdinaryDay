using UnityEngine;
using System.Collections;

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


    public IEnumerator SetText(string text)
    {
        yield return _multiPageText.SetText(text);
    }


    public IEnumerator AppendText(string text)
    {
        yield return _multiPageText.AppendText(text);
    }
}
