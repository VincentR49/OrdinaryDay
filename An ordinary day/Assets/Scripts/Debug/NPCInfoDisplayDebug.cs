using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class NPCInfoDisplayDebug : MonoBehaviour
{
    private static string Separator = new String('-', 35);
    [SerializeField]
    private NPCDataList _allNPCList;
    [SerializeField]
    private Dropdown _dropdown;
    [SerializeField]
    private Text _text;

    private NPCData _npc => _dropdown.value == _allNPCList.Items.Count ? null : _allNPCList.Items[_dropdown.value];

    private void Awake()
    {
        _dropdown.ClearOptions();
        var options = new List<Dropdown.OptionData>();
        foreach (var npc in _allNPCList.Items)
            options.Add(new Dropdown.OptionData(npc.FirstName));
        options.Add(new Dropdown.OptionData("None"));
        _dropdown.AddOptions(options);
    }


    private void Update()
    {
        _text.text = GetDebugText();
    }


    private string GetDebugText()
    {
        if (_npc == null)
            return "";
        var sb = new StringBuilder();
        sb.AppendLine(_npc.FirstName + " " + _npc.LastName);
        var lastKnownPosition = _npc.InGameSchedule.GetLastKnownPosition();
        sb.AppendLine("Scene: " + (lastKnownPosition == null ? "Unknown"
            : Path.GetFileNameWithoutExtension(lastKnownPosition.ScenePath)));
        sb.AppendLine("Last know position: " + (lastKnownPosition == null ? "Unknown"
            : lastKnownPosition.Position.ToString()));
        sb.AppendLine(GetScheduleText());
        return sb.ToString();
    }


    private string GetScheduleText()
    {
        var sb = new StringBuilder();
        foreach (var task in _npc.InGameSchedule.Value.Tasks)
            sb.AppendLine(task.ToString());
        return sb.ToString();
    }
}
