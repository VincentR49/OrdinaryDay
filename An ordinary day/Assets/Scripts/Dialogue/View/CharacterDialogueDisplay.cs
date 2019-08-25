using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manage the display of a dialogue for a given character
/// </summary>
public abstract class CharacterDialogueDisplay : MonoBehaviour
{
    private static KeyCode GoToNextPageKey = KeyCode.Space;

    [SerializeField]
    protected GameObject _container;
    [SerializeField]
    protected TextMeshProUGUI _dialogue;
    [SerializeField]
    protected GameObject _nextPage;
    [SerializeField]
    protected TextMeshProUGUI _name;
    [SerializeField]
    protected Image _picture;

    private int TotalPage => _dialogue.textInfo.pageCount;


    private void Update()
    {
        if (Input.GetKeyDown(GoToNextPageKey) && !IsAtLastPage())
        {
            GoToNextPage();
        }
    }


    public void Show(bool show)
    {
        _container.SetActive(show);
    }


    public IEnumerator SetLine(string text)
    {
        Debug.Log("Set text: " + text);
        _nextPage.SetActive(false);
        _dialogue.text = text;
        _dialogue.pageToDisplay = 1;
        yield return new WaitForEndOfFrame();
        UpdateNextPageDisplay();
    }


    public virtual void Reset()
    {
        _dialogue.text = "";
        _dialogue.pageToDisplay = 1;
        _nextPage.SetActive(false);
    }


    public IEnumerator AppendLine(string textToDisplay)
    {
        if (string.IsNullOrEmpty(_dialogue.text))
        {
            SetLine(textToDisplay);
            yield break;
        } 
        _dialogue.text += System.Environment.NewLine;
        _dialogue.text += textToDisplay;
        yield return new WaitForEndOfFrame();
        UpdateNextPageDisplay();
    }


    // Link via inspector to the next button
    // TODO will dispappear
    public void GoToNextPage()
    {
        _dialogue.pageToDisplay += 1;
        UpdateNextPageDisplay();
    }


    public bool IsAtLastPage() => TotalPage == 0 || _dialogue.pageToDisplay >= TotalPage;


    private void UpdateNextPageDisplay()
    {
        Debug.Log("Current page: " + _dialogue.pageToDisplay + ". Total page: " + TotalPage);
        _nextPage.SetActive(!IsAtLastPage());
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
