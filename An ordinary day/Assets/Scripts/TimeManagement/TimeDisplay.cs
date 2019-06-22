using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    [Header("Colors")]

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
        _text.text = WorldClock.GetTime().ToString("MM/dd/yyyy HH:mm:ss");
        _text.color = WorldClock.IsPaused ? _pauseColor : _standardColor;
    }
}
