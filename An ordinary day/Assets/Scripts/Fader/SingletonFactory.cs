using UnityEngine;

public class SingletonFactory : Singleton<SingletonFactory>
{
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
