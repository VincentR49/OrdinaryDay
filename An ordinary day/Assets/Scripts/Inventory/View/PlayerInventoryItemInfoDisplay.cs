using UnityEngine;
using System;
using TMPro;

/// <summary>
/// Manage the display of secondary information about an inventory item (name / description)
/// </summary>
public class PlayerInventoryItemInfoDisplay : MonoBehaviour
{
    private static string Separator = new String('-', 20);
    [SerializeField]
    private TextMeshProUGUI _text;

    private GameItemData _itemData;

    public void Init(GameItemData itemData)
    {
        _itemData = itemData;
        _text.text = GetText();
    }


    private string GetText()
    {
        var text = string.Format("{0}\n" +
            "{1}\n" +
            "{2}", _itemData.Name, Separator, _itemData.Description);
        return text;
    }


    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }
}
