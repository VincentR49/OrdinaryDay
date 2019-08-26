using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manage the display of a dialogue for a given character
/// </summary>
public abstract class CharacterDialogueDisplay : MonoBehaviour
{
    private static KeyCode ContinueKey = KeyCode.Space;

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
    private int Page => _dialogue.pageToDisplay;
    private int CurrentPageEndIndex => _dialogue.textInfo.pageInfo[Page - 1].lastCharacterIndex;
    public bool IsDisplayingText { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(ContinueKey))
        {
            if (IsDisplayingText)
            {
                return;
            }
            if(! IsAtLastPage())
            {
                StartCoroutine(GoToNextPage());
            }
        }
    }

    public void Show(bool show)
    {
        _container.SetActive(show);
    }


    public IEnumerator SetLine(string text)
    {
        Debug.Log("Set text: " + text);
        _dialogue.text = text;
        _dialogue.pageToDisplay = 1;
        _dialogue.maxVisibleCharacters = 0;
        yield return ShowTextProgressively();
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
        _nextPage.SetActive(false);
        _dialogue.text += System.Environment.NewLine;
        _dialogue.text += textToDisplay;
        yield return ShowTextProgressively();
    }


    private IEnumerator ShowTextProgressively()
    {
        IsDisplayingText = true;
        _nextPage.SetActive(false);
        yield return new WaitForEndOfFrame();
        while (_dialogue.maxVisibleCharacters <= CurrentPageEndIndex)
        {
            _dialogue.maxVisibleCharacters++;
            yield return new WaitForEndOfFrame();
        }
        IsDisplayingText = false;
        RefreshNextPageDisplay();
    }


    public IEnumerator GoToNextPage()
    {
        _dialogue.pageToDisplay += 1;
        yield return ShowTextProgressively();
    }


    public bool IsAtLastPage() => TotalPage == 0 || Page >= TotalPage;


    private void RefreshNextPageDisplay()
    {
        Debug.Log("Current page: " + _dialogue.pageToDisplay + ". Total page: " + TotalPage);
        _nextPage.SetActive(!IsAtLastPage());
    }

    public bool IsActive() => isActiveAndEnabled;
}
