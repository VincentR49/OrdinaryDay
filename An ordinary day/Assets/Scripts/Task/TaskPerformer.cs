using UnityEngine;

/// <summary>
/// Behaviour that handle task
/// </summary>
public class TaskPerformer : MonoBehaviour
{
    [SerializeField]
    private TargetReacher _targetReacher;

    private TaskPerformerDelegate _performer;

    
    public void Perform(Task task)
    {
        Debug.Log("Perform " + task.Description);
        if (task is Move)
        {
            var performer = new MovePerformer();
            performer.Perform((Move) task, _targetReacher);
            _performer = performer; // cast
            _performer.OnTaskFinished += OnTaskFinished;
        }
    }



    private void OnTaskFinished()
    {
        _performer.OnTaskFinished -= OnTaskFinished;
        Debug.Log("TaskPerformer: OnTaskFinished");
    }
}
