using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class CharacterDialogueDisplay : MonoBehaviour
{
    [SerializeField]
    protected GameObject _container;
    [SerializeField]
    protected TextMeshProUGUI _dialogue;
    [SerializeField]
    protected Button _nextButton;
    [SerializeField]
    protected TextMeshProUGUI _name;
    [SerializeField]
    protected Image _picture;

    private int TotalPage => _dialogue.textInfo.pageCount;


    public void Show(bool show)
    {
        _container.SetActive(show);
    }


    public IEnumerator SetText(string text)
    {
        _nextButton.gameObject.SetActive(false);
        _dialogue.text = text;
        _dialogue.pageToDisplay = 1;
        yield return null;
        UpdateNextPageDisplay();
    }

    // Make it routine (IEnumerator)
    public IEnumerator AppendText(string textToDisplay)
    {
        _nextButton.gameObject.SetActive(false);
        _dialogue.text += textToDisplay;
        yield return null;
        UpdateNextPageDisplay();
    }


    // Link via inspector to the next button
    public void GoToNextPage()
    {
        _dialogue.pageToDisplay += 1;
        UpdateNextPageDisplay();
    }


    private void UpdateNextPageDisplay()
    {
        Debug.Log("Total page: " + TotalPage);
        _nextButton.gameObject.SetActive(_dialogue.pageToDisplay < TotalPage);
    }


    public void SetCharacterName(string characterName)
    {
        _name.text = characterName;
    }

    public void SetCharacterPicture(Sprite sprite)
    {
        _picture.sprite = sprite;
    }

    public bool IsActive() => isActiveAndEnabled;
}
