﻿using UnityEngine;

/// <summary>
/// Behaviour that handle task related to the current gameObject.
/// Attach to the gameobject that should perform the task
/// Delegate the work to TaskPerformerHandler that handles a specific task type.
/// </summary>
/// 
// TODO delegate the specific task implementation ?
public class TaskPerformer : BasicTaskPerformer
{
    [SerializeField]
    private TargetReacher _targetReacher;

    private Spawner _currentSpawnPoint;
    private Vector2 _currentMoveTarget;

    private void Awake()
    {
        _targetReacher.OnTargetReached += OnMoveToDestinationSucceded;
    }


    private void OnDestroy()
    {
        _targetReacher.OnTargetReached -= OnMoveToDestinationSucceded;
    }


    #region Override
    public override void Perform(Task task, float maxDurationSec, float initProgressPrc)
    {
        base.Perform(task, maxDurationSec, initProgressPrc);
        switch (task)
        {
            case Move move:
                PerformMove(move);
                break;
            case SpawnNPC spawn:
                PerformSpawn(spawn);
                break;
            default:
                Debug.LogError("Task not implemented yet: " + task);
                break;
        }
    }


	public override void Stop()
	{
		switch (_task)
        {
            case Move move:
                CancelMove();
                break;
            default:
                // do nothing
                break;
        }
		base.Stop();
	}


	protected override void OnCurrentTaskDurationReachedLimit()
    {
        OnTaskFailed(TaskFailedConstants.CouldntFinishOnTime);
    }


    protected override void RefreshCurrentTaskProgress()
    {
        switch (_task)
        {
            case Move move:
                ProgressPrc = _initProgressPrc + _targetReacher.ProgressPrc  * (1 - _initProgressPrc);
                break;
            default:
                // do nothing
                break;
        }
    }
    #endregion

    #region Move
    private void PerformMove(Move task)
    {
        if (!task.Destination.IsInCurrentScene())
        {
            OnTaskFailed(TaskFailedConstants.NotInGoodScene);
            return;
        }
        _currentMoveTarget = task.Destination.Value.Position;
        _targetReacher.GoToTarget(_currentMoveTarget);
    }


    private void CancelMove()
	{
        _targetReacher.Stop();
    }


    private void OnMoveToDestinationSucceded(Vector2 target)
    {
        if (_currentMoveTarget == target)
            OnTaskFinished();
    }
    #endregion


    #region Spawn
    public void PerformSpawn(SpawnNPC spawn)
    {
        if (!spawn.IsInCurrentScene()) // Destroy the current npc
        {
            OnTaskFinished();
            Destroy(gameObject);
            return;
        }
        Debug.LogError("Spawn npc already present on the scene (teleportation). Shouldnt be here.");
        OnTaskFinished();
    }
    #endregion
}
