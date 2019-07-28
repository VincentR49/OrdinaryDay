/// <summary>
/// Simulated task Performer.
/// Basic TaskPerformer that ends automatically a task after reaching the defined duration.
/// </summary>
public class SimulatedTaskPerformer : BasicTaskPerformer
{
    protected override void OnCurrentTaskDurationReachedLimit()
    {
        OnTaskFinished();
    }
}
