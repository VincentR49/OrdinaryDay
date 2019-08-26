﻿using UnityEngine;
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

    public static NPCController Get(NPCData npcData)
    {
        foreach (var controller in _npcControllers)
        {
            if (controller._npcData == npcData)
                return controller;
        }
        return null;
    }

    // Call when a dialogue is starting
    public void OnDialogueStarted(Transform speakerTransform)
    {
        Debug.Log("On dialogue started: " + _npcData.FirstName);
        StartCoroutine(StopWalkingAndFaceSpeaker(speakerTransform));
    }


    private IEnumerator StopWalkingAndFaceSpeaker(Transform speakerTransform)
    {
        _walkManager.Stop();
        yield return new WaitForEndOfFrame();
        // more polite ;)
        _spriteDirectioner.FaceTowards(speakerTransform);
    }


    public static List<NPCController> GetNPCControllers() => _npcControllers;

    public NPCData GetNPCData() => _npcData;
}
