﻿using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manage the PNJ behaviours
/// </summary>
public class PNJController : MonoBehaviour
{
    [Header("Trackers")]
    [SerializeField]
    private PositionTracker _positionTracker;

    [Header("Managers")]
    [SerializeField]
    private WalkManager _walkManager;
    [SerializeField]
    private SpriteDirectioner _spriteDirectioner;
    [SerializeField]
    private ScheduleHandler _scheduleHandler;
    
    [Header("Debug")]
    [SerializeField]
    private PNJData _pnjData;
    [SerializeField]
    private bool _initOnStart;

    private bool _isInit;
    private static List<PNJController> _pnjControllers = new List<PNJController>();
    public delegate void InstancesChangedHandler(PNJController pnj);
    public static event InstancesChangedHandler OnPNJAdded;
    public static event InstancesChangedHandler OnPNJRemoved;

    private void OnDestroy()
    {
        _pnjControllers.Remove(this);
        OnPNJRemoved?.Invoke(this);
    }


    private void Start()
    {
        if (_initOnStart)
            Init(_pnjData);
    }


    private void Update()
    {
        if (!_isInit)
            return;
        _positionTracker.UpdatePosition(transform.position);
    }

    #region Init
    public void Init(PNJData pnjData)
    {
        _pnjData = pnjData;
        Debug.Log("PNJ Initialisation: " + _pnjData);
        InitSprites();
        InitScheduleSystem();
        _positionTracker.Init(pnjData.PositionTracking);
        if (!_pnjControllers.Contains(this))
        {
            _pnjControllers.Add(this); // we register only if initialized 
            OnPNJAdded?.Invoke(this);
        }
        _isInit = true;
    }


    private void InitSprites()
    {
        _spriteDirectioner.SetCardinalSprite(_pnjData.CardinalSprite);
        _walkManager.SetWalkAnimation(_pnjData.WalkingAnimation);
    }


    private void InitScheduleSystem()
    {
        _scheduleHandler.Init(_pnjData.InGameSchedule);
    }
    #endregion

    public static PNJController Get(PNJData pnjData)
    {
        foreach (var controller in _pnjControllers)
        {
            if (controller._pnjData == pnjData)
                return controller;
        }
        return null;
    }

    public PNJData GetPNJData() => _pnjData;
}
