using UnityEngine;

public class MovePerformer : TaskPerformerDelegate
{
    private TargetReacher _targetReacher;
    public override event OnTaskFinishedHandler OnTaskFinished; // necessary to be called here

    public void Perform(Move task, TargetReacher targetReacher)
    {
        base.Perform(task);
        _targetReacher = targetReacher;
        // TODO check scene
        _targetReacher.OnTargetReached += OnMoveToDestinationSucceded;
        _targetReacher.GoToTarget(task.Destination.Value.Position);
    }


    private void OnMoveToDestinationSucceded(Vector2 target)
    {
        _targetReacher.OnTargetReached -= OnMoveToDestinationSucceded;
        Debug.Log("OnMoveToDestinationSucceded");
        OnTaskFinished?.Invoke();
    }
}
