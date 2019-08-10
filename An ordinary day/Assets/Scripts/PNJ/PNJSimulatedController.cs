using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage a simulated PNJ behaviour (not present on current scene)
/// </summary>
public class PNJSimulatedController : MonoBehaviour
{
    [SerializeField]
    private ScheduleHandler _scheduleHandler;
    [SerializeField]
    private PathFinder _pathFinder;

    private PNJData _pnjData;
    private PNJController InSceneController => PNJController.Get(_pnjData);

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
            var doingTask = _scheduleHandler.GetDoingTask();
            // If performing move, special behaviour
            // we estimated where the pnj should be based on the current progress of the task and the static path to follow
            if (doingTask != null && doingTask.Task is Move)
            {
                // todo put inside specific method
                // todo store the current following path inside a runtime Scriptable??
                InitPathFindingSystem();
                Debug.Log("[PNJSimulatedController] Was performing moving task. Set new position.");
                // todo compute the position
                var move = (Move) doingTask.Task;
                var staticPathToDo = _pathFinder.FindShortestPath(lastPosition.Position,
                                            move.Destination.Value.Position, out bool couldReachTarget, true);
                int nNodes = staticPathToDo.Count;
                if (nNodes > 0)
                {
                    int startPositionIndex = (int) Math.Round(doingTask.CompletionPrc * nNodes);
                    startPositionIndex = Mathf.Clamp(startPositionIndex, 0, nNodes - 1);
                    var startPosition = staticPathToDo[startPositionIndex];
                    lastPosition.Position = startPosition; // assign the accurate position to the spawn position
                }
            }
            PNJInstancier.InstanciatePNJ(_pnjData, lastPosition.Position, Direction.South);
        }
    }


    private void InitPathFindingSystem()
    {
        var colliderScanner = FindObjectOfType<ColliderScanner>();
        if (colliderScanner == null)
        {
            Debug.LogError("Collider scanner not found !");
            return;
        }
        _pathFinder.SetColliderScanner(colliderScanner);
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
