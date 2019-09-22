using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Use this class to instanciate a prefab based game object which will be persistant among scene.
/// Usefull for object like screenFader, gameController etc.
/// Allow to avoid to duplicate those objects in all the game scenes.
/// </summary>
public class SingletonFactory : Singleton<SingletonFactory>
{
    private Dictionary<string, GameObject> _singletonInstances = new Dictionary<string, GameObject>();

    public static GameObject ScreenFader() => GetInstance(PathConstants.ScreenFaderPrefab);
    public static GameObject TimeManager() => GetInstance(PathConstants.TimeManagerPrefab);
    public static GameObject GameController() => GetInstance(PathConstants.GameControllerPrefab);


    public static GameObject GetInstance(string prefabPath)
    {
        return Instance.InstanciatePrefabIfNeeded(prefabPath);
    }

    private GameObject InstanciatePrefabIfNeeded(string prefabPath)
    {
        if (_singletonInstances.ContainsKey(prefabPath))
            return _singletonInstances[prefabPath];
        var prefab = Utils.GetPrefab(prefabPath);
        var go = Instantiate(prefab, Instance.transform);
        _singletonInstances.Add(prefabPath, go);
        return go;
    }
}
