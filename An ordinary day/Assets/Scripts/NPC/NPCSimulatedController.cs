using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage a simulated NPC behaviour (not present on current scene)
/// </summary>
public class NPCSimulatedController : MonoBehaviour
{
    [SerializeField]
    private ScheduleHandler _scheduleHandler;
    [SerializeField]
    private PathFinder _pathFinder;

    private NPCData _npcData;
    private NPCController InSceneController => NPCController.Get(_npcData);

    private void Awake()
    {
        NPCController.OnNPCAdded += OnNPCCreated;
        NPCController.OnNPCRemoved += OnNPCDestroyed;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // todo probably move to somewhere else
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        // check if we should instanciate the npc
        Debug.Log("On scene loaded: " + arg0.path);
        var lastPosition = _scheduleHandler.GetLastKnownPosition();
        if (lastPosition != null && lastPosition.IsInCurrentScene())
        {
            var doingTask = _scheduleHandler.GetDoingTask();
            // If performing move, special behaviour
            // we estimated where the npc should be based on the current progress of the task and the static path to follow
            if (doingTask != null && doingTask.Task is Move)
            {
                // todo put inside specific method
                // todo store the current following path inside a runtime Scriptable??
                InitPathFindingSystem();
                Debug.Log("[NPCSimulatedController] Was performing moving task. Set new position.");
                // todo compute the position
                var move = (Move)doingTask.Task;
                var staticPathToDo = _pathFinder.FindShortestPath(lastPosition.Position,
                                            move.Destination.Value.Position, out bool couldReachTarget, true);
                int nNodes = staticPathToDo.Count;
                if (nNodes > 0)
                {
                    int startPositionIndex = (int)Math.Round(doingTask.CompletionPrc * nNodes);
                    startPositionIndex = Mathf.Clamp(startPositionIndex, 0, nNodes - 1);
                    var startPosition = staticPathToDo[startPositionIndex];
                    lastPosition.Position = startPosition; // assign the accurate position to the spawn position
                }
            }
            NPCInstancier.InstanciateNPC(_npcData, lastPosition.Position, Direction.South);
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
        NPCController.OnNPCAdded -= OnNPCCreated;
        NPCController.OnNPCRemoved -= OnNPCDestroyed;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void Init(NPCData npc)
    {
        _npcData = npc;
        npc.InitRuntimeSchedule();
        _scheduleHandler.Init(npc.InGameSchedule);
        if (NPCController.Get(_npcData) == null)
            Enable();
        else
            Disable();
    }


    public void OnNPCCreated(NPCController npc)
    {
        if (npc.GetNPCData() == _npcData)
            Disable();
    }


    public void OnNPCDestroyed(NPCController npc)
    {
        if (npc.GetNPCData() == _npcData)
            Enable();
    }


    private void Enable()
    {
        Debug.Log("[NPCSimulatedController] Enable Simulated Controller: " + _npcData);
        _scheduleHandler.Resume();
    }


    private void Disable()
    {
        Debug.Log("[NPCSimulatedController] Disable Simulated Controller: " + _npcData);
        _scheduleHandler.Stop();
    }

    private void ResetNPCSchedules()
    {
        _npcData.InGameSchedule.Reset();
    }


    // Attach in inspector to a game event listener
    public void OnTimeLoopInit()
    {
        ResetNPCSchedules();
    }
}
