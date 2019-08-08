using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class PNJInfoDisplayDebug : MonoBehaviour
{
    private static string Separator = new String('-', 35);
    [SerializeField]
    private PNJDataList _allPNJList;
    [SerializeField]
    private Dropdown _dropdown;
    [SerializeField]
    private Text _text;

    private PNJData _pnj => _dropdown.value == _allPNJList.Items.Count ? null : _allPNJList.Items[_dropdown.value];

    private void Awake()
    {
        _dropdown.ClearOptions();
        var options = new List<Dropdown.OptionData>();
        foreach (var pnj in _allPNJList.Items)
            options.Add(new Dropdown.OptionData(pnj.FirstName));
        options.Add(new Dropdown.OptionData("None"));
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
        var sb = new StringBuilder();
        var text = string.Format("" +
            "{2} {3} {0}" +
            "{4} -> {5} {0}" +
            "{1}", Environment.NewLine, GetScheduleText(), _pnj.FirstName, _pnj.LastName,
                          Path.GetFileNameWithoutExtension(_pnj.PositionTracking.GamePosition.ScenePath),
                          _pnj.PositionTracking.GamePosition.Position);
        return text;
    }


    private string GetScheduleText()
    {
        var sb = new StringBuilder();
        foreach (var task in _pnj.InGameSchedule.Value.Tasks)
            sb.AppendLine(task.ToString());
        return sb.ToString();
    }
}
