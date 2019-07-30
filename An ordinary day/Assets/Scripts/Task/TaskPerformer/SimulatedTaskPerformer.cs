/// <summary>
/// Simulated task Performer.
/// Basic TaskPerformer that ends automatically a task after reaching the defined duration.
/// </summary>
public class SimulatedTaskPerformer : BasicTaskPerformer
{
    // Finish task when we reach the limit duration
    protected override void OnCurrentTaskDurationReachedLimit()
    {
        OnTaskFinished();
    }

    protected override void RefreshCurrentTaskProgress()
    {
        ProgressPrc = _currentTaskDurationSec / _maxDurationTask;
    }
}
