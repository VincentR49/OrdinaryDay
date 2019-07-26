using UnityEngine;

/// <summary>
/// Perform a moving task
/// </summary>
public class MovePerformer : TaskPerformerDelegate
{
    private const string NotInGoodScene = "notInGoodScene";

    private TargetReacher _targetReacher;


    public void Perform(Move task, TargetReacher targetReacher)
    {
        base.Perform(task);
        if (!task.Destination.IsInCurrentScene())
        {
            OnTaskFailed(NotInGoodScene);
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


    public override void CleanListeners()
    {
        _targetReacher.OnTargetReached -= OnMoveToDestinationSucceded;
    }

    public override void Cancel()
    {
        base.Cancel();
        _targetReacher.Stop();
    }
}
