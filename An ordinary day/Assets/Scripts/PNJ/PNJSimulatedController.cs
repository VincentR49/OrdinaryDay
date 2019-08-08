using UnityEngine;

/// <summary>
/// Manage a simulated PNJ behaviour (not present on current scene)
/// </summary>
public class PNJSimulatedController : MonoBehaviour
{
    [SerializeField]
    private SimulatedTaskPerformer _taskPerformer;
    [SerializeField]
    private ScheduleHandler _scheduleHandler;
    [SerializeField]
    private PositionTracker _positionTracker;

    private PNJData _pnjData;


    private void Awake()
    {
        PNJController.OnPNJAdded += OnPNJCreated;
        PNJController.OnPNJRemoved += OnPNJDestroyed;
    }


    private void OnDestroy()
    {
        PNJController.OnPNJAdded -= OnPNJCreated;
        PNJController.OnPNJRemoved -= OnPNJDestroyed;
    }


    public void Init(PNJData pnj)
    {
        _pnjData = pnj;
        pnj.InitRuntimeSchedule();
        _scheduleHandler.Init(pnj.InGameSchedule);
        _positionTracker.Init(pnj.PositionTracking);
        Enable(PNJController.Get(_pnjData) == null);
    }


    public void OnPNJCreated(PNJController pnj)
    {
        if (pnj.GetPNJData() == _pnjData)
            Enable(false);
    }


    public void OnPNJDestroyed(PNJController pnj)
    {
        if (pnj.GetPNJData() == _pnjData)
            Enable(false);
    }


    private void Enable(bool enable)
    {
        gameObject.SetActive(enable);
    }


    private void ResetPNJSchedules()
    {
        _pnjData.InGameSchedule.Reset();
    }


    // Attach in inspector to a game event listener
    public void OnTimeLoopStarted()
    {
        ResetPNJSchedules();
    }
}
