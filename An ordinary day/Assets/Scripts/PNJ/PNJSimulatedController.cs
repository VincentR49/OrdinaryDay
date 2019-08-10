using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage a simulated PNJ behaviour (not present on current scene)
/// </summary>
public class PNJSimulatedController : MonoBehaviour
{
    [SerializeField]
    private ScheduleHandler _scheduleHandler;

    private PNJData _pnjData;

    private void Awake()
    {
        PNJController.OnPNJAdded += OnPNJCreated;
        PNJController.OnPNJRemoved += OnPNJDestroyed;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // todo probably move to somewhere else
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        // check if we should instanciate the pnj
        Debug.Log("On scene loaded: " + arg0.path);
        var lastPosition = _scheduleHandler.GetLastKnownPosition();
        if (lastPosition != null && lastPosition.IsInCurrentScene())
        {
            // define direction later
            PNJInstancier.InstanciatePNJ(_pnjData, lastPosition.Position, Direction.South);
        }
    }

    private void OnDestroy()
    {
        PNJController.OnPNJAdded -= OnPNJCreated;
        PNJController.OnPNJRemoved -= OnPNJDestroyed;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void Init(PNJData pnj)
    {
        _pnjData = pnj;
        pnj.InitRuntimeSchedule();
        _scheduleHandler.Init(pnj.InGameSchedule);
        if (PNJController.Get(_pnjData) == null)
            Enable();
        else
            Disable();
    }


    public void OnPNJCreated(PNJController pnj)
    {
        if (pnj.GetPNJData() == _pnjData)
            Disable();
    }


    public void OnPNJDestroyed(PNJController pnj)
    {
        if (pnj.GetPNJData() == _pnjData)
            Enable();
    }


    private void Enable()
    {
        Debug.Log("[PNJSimulatedController] Enable Simulated Controller: " + _pnjData);
        _scheduleHandler.enabled = true;
    }


    private void Disable()
    {
        Debug.Log("[PNJSimulatedController] Disable Simulated Controller: " + _pnjData);
        _scheduleHandler.Stop();
        _scheduleHandler.enabled = false;
    }

    private void ResetPNJSchedules()
    {
        _pnjData.InGameSchedule.Reset();
    }


    // Attach in inspector to a game event listener
    public void OnTimeLoopInit()
    {
        ResetPNJSchedules();
    }
}
