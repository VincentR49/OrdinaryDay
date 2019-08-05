using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField]
    private Text _text = default;
    [SerializeField]
    private RuntimeDateTime _currentTime = default;
    [SerializeField]
    private Color _pauseColor = Color.red;

    private Color _standardColor;


    private void Awake()
    {
        _standardColor = _text.color;
    }

    private void Update()
    {
        Refresh();
    }


    private void Refresh()
    {
        _text.text = _currentTime.Value.ToString("MM/dd/yyyy HH:mm:ss");
        _text.color = GamePauser.IsPaused ? _pauseColor : _standardColor;
    }
}
