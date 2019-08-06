using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class PNJInfoDisplayDebug : MonoBehaviour
{
    private static string Separator = new String('-', 35);
    [SerializeField]
    private PNJDataList _allPNJList;
    [SerializeField]
    private Dropdown _dropdown;
    [SerializeField]
    private Text _text;

    private PNJData _pnj => _dropdown.value == 0 ? null : _allPNJList.Items[_dropdown.value - 1];

    private void Awake()
    {
        _dropdown.ClearOptions();
        var options = new List<Dropdown.OptionData>();
        options.Add(new Dropdown.OptionData("None"));
        foreach (var pnj in _allPNJList.Items)
            options.Add(new Dropdown.OptionData(pnj.FirstName));
        _dropdown.AddOptions(options);
    }


    private void Update()
    {
        _text.text = GetPNJDebugText();
    }


    private string GetPNJDebugText()
    {
        if (_pnj == null)
            return "";
        var text = string.Format("" +
            "{2} {3} {0}" +
            "{1} {0}" +
            "{4} -> {5}", Environment.NewLine, Separator, _pnj.FirstName, _pnj.LastName,
                          _pnj.PositionTracking.LastPosition.ScenePath,
                          _pnj.PositionTracking.LastPosition.Position);
        return text;
    }
}
