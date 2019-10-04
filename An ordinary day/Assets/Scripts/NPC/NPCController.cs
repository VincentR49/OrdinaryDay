using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Manage the NPC behaviours
/// </summary>
public class NPCController : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField]
    private WalkManager _walkManager;
    [SerializeField]
    private SpriteDirectioner _spriteDirectioner;
    [SerializeField]
    private ScheduleHandler _scheduleHandler;
    [SerializeField]
    private DialogueWithPlayerAgent _speakableObject;

    [Header("Dialogue")]
    [SerializeField]
    private NPCDialogueDisplay _dialogueDisplay;


    [Header("Debug")]
    [SerializeField]
    private NPCData _npcData;
    [SerializeField]
    private bool _initOnStart;

    private static List<NPCController> _npcControllers = new List<NPCController>();
    public delegate void InstancesChangedHandler(NPCController npc);
    public static event InstancesChangedHandler OnNPCAdded;
    public static event InstancesChangedHandler OnNPCRemoved;


    private void OnDestroy()
    {
        _npcControllers.Remove(this);
        OnNPCRemoved?.Invoke(this);
    }


    private void Start()
    {
        if (_initOnStart)
            Init(_npcData);
    }

    #region Init
    public void Init(NPCData npcData)
    {
        _npcData = npcData;
        name = npcData.FirstName;
        Debug.Log("NPC Initialisation: " + _npcData);
        _dialogueDisplay.Show(false);
        _speakableObject.SetDialogueData(npcData.DialogueAgentData);
        InitSprites();
        InitScheduleSystem();
        if (!_npcControllers.Contains(this))
        {
            _npcControllers.Add(this); // we register only if initialized 
            OnNPCAdded?.Invoke(this);
        }
    }


    private void InitSprites()
    {
        _spriteDirectioner.Init(_npcData.CardinalSprite);
        _walkManager.Init(_npcData.WalkingAnimation);
    }


    private void InitScheduleSystem()
    {
        _scheduleHandler.Init(_npcData.InGameSchedule);
    }
    #endregion


    #region Accessors

    public static List<NPCController> GetNPCControllers() => _npcControllers;
    public NPCData GetNPCData() => _npcData;


    public static NPCController Get(NPCData npcData)
    {
        foreach (var controller in _npcControllers)
        {
            if (controller._npcData == npcData)
                return controller;
        }
        return null;
    }
    #endregion
}
