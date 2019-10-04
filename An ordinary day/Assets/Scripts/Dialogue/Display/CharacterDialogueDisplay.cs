using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manage the display of a dialogue for a given character
/// </summary>
public class CharacterDialogueDisplay : MonoBehaviour
{
    private static KeyCode ContinueKey = KeyCode.Space;

    [SerializeField]
    protected GameObject _container;
    [SerializeField]
    protected TextMeshProUGUI _name;
    [SerializeField]
    protected Image _picture;

    [Header("Text")]
    [SerializeField]
    protected MultiPageTextHandler _multiPageText;

    public bool IsAtLastPage => _multiPageText.IsAtLastPage();
    public bool IsDisplayingText => _multiPageText.IsDisplayingText;
    
    /// <summary>
    /// To call before displaying text
    /// Initialize the picture and name to diaply during dialogue.
    /// </summary>
    /// <param name="picture"></param>
    /// <param name="nameText"></param>
    public void Init(Sprite picture, string nameText = "")
    {
        _picture.sprite = picture;
        _name.text = nameText;
    }


    public IEnumerator SetText(string text)
    {
        yield return _multiPageText.SetText(text);
    }

    public IEnumerator AppendText(string text)
    {
        yield return _multiPageText.AppendText(text);
    }

        public void Reset()
    {
        _multiPageText.Reset();
    }


    private void Update()
    {
        if (Input.GetKeyDown(ContinueKey))
        {
            if (IsDisplayingText)
            {
                return;
            }
            if (!IsAtLastPage)
            {
                StartCoroutine(_multiPageText.GoToNextPage());
            }
        }
    }


    public void Show(bool show)
    {
        _container.SetActive(show);
    }

    public bool IsActive() => isActiveAndEnabled;
}
