using UnityEngine;
using UnityEngine.UI;

public class LoadingDisplay : MonoBehaviour
{
    [SerializeField]
    private Text _text;


    private void Update()
    {
        //Debug.Log(SceneLoaderAsync.Instance.LoadingProgress);
        _text.text = string.Format("Loading: {0}%", SceneLoader.LoadingProgress);
    }
}
