using UnityEngine;

/// <summary>
/// Use this class to instanciate a prefab based game object which will be persistant among scene.
/// Usefull for object like screenFader, gameController etc.
/// Allow to avoid to duplicate those objects in all the game scenes.
/// </summary>
public class SingletonFactory : Singleton<SingletonFactory>
{
    // TODO find more generic way to write this (can be messy with a lot of prefabs)
    private static GameObject _screenFaderPrefab = Utils.GetPrefab(PathConstants.ScreenFaderPrefab);
    private static GameObject _screenFaderInstance;

    public static GameObject ScreenFader()
    {
        if (_screenFaderInstance == null)
        {
            Debug.Log("Instanciate Screen Fader");
            _screenFaderInstance = Instantiate(_screenFaderPrefab, Instance.transform);
        }
        return _screenFaderInstance;
    }
}
