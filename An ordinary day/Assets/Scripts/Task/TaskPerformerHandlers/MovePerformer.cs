using UnityEngine;

/// <summary>
/// Perform a move from a given move task
/// </summary>
public class MovePerformer : TaskPerformerHandler
{
    private TargetReacher _targetReacher;

    public void Perform(Move task, TargetReacher targetReacher)
    {
        base.Perform(task);
        if (!task.Destination.IsInCurrentScene())
        {
            OnTaskFailed(TaskFailedConstants.NotInGoodScene);
            return;
        }
        _targetReacher = targetReacher;
        _targetReacher.OnTargetReached += OnMoveToDestinationSucceded;
        _targetReacher.GoToTarget(task.Destination.Value.Position);
    }


    private void OnMoveToDestinationSucceded(Vector2 target)
    {
        Debug.Log("OnMoveToDestinationSucceded");
        OnTaskFinished();
    }


    protected override void Clean()
    {
        _targetReacher.OnTargetReached -= OnMoveToDestinationSucceded;
        base.Clean();
    }

    public override void Cancel()
    {
        _targetReacher.Stop();
        base.Cancel();
    }
}
