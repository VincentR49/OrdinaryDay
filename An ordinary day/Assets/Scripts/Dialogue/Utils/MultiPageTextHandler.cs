using System.Collections;
using TMPro;
using UnityEngine;

public class MultiPageTextHandler : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI _text;
    [SerializeField]
    private bool _showProgressively = true; // todo manage better
    
    [Header("Optional")]
    [SerializeField]
    protected GameObject _nextPage;

    private int TotalPage => _text.textInfo.pageCount;
    private int Page => _text.pageToDisplay;
    private int CurrentPageEndIndex => _text.textInfo.pageInfo[Page - 1].lastCharacterIndex;
    public bool IsDisplayingText { get; private set; }


    public IEnumerator SetText(string text)
    {
        //Debug.Log("Set text: " + text);
        ShowNextPageIndicator(false);
        _text.text = text;
        _text.pageToDisplay = 1;
        _text.maxVisibleCharacters = 0;
        yield return ShowTextProgressively();
    }


    public IEnumerator AppendText(string text)
    {
        ShowNextPageIndicator(false);
        if (string.IsNullOrEmpty(_text.text))
        {
            SetText(text);
            yield break;
        }
        _text.text += System.Environment.NewLine;
        _text.text += text;
        yield return ShowTextProgressively();
    }


    public void Reset()
    {
        _text.text = "";
        _text.pageToDisplay = 1;
        ShowNextPageIndicator(false);
    }


    private IEnumerator ShowTextProgressively()
    {
        IsDisplayingText = true;
        yield return new WaitForEndOfFrame();
        if (_showProgressively)
        {
            while (_text.maxVisibleCharacters <= CurrentPageEndIndex)
            {
                _text.maxVisibleCharacters++;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            _text.maxVisibleCharacters = CurrentPageEndIndex;
        }
        IsDisplayingText = false;
        RefreshNextPageDisplay();
    }


    public IEnumerator GoToNextPage()
    {
        _text.pageToDisplay += 1;
        ShowNextPageIndicator(false);
        yield return ShowTextProgressively();
    }


    public bool IsAtLastPage() => TotalPage == 0 || Page >= TotalPage;


    private void RefreshNextPageDisplay()
    {
        //Debug.Log("Current page: " + _dialogue.pageToDisplay + ". Total page: " + TotalPage);
        ShowNextPageIndicator(!IsAtLastPage());
    }

   
    private void ShowNextPageIndicator(bool show)
    {
        if (_nextPage)
        {
            _nextPage.SetActive(show);
        }
    }
}
