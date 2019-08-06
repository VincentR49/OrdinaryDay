using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    private float _count;
    
    private IEnumerator Start()
    {
        GUI.depth = 2;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            _count = (1 / Time.deltaTime);
            _text.text = "FPS: " + (Mathf.Round(_count));
            yield return new WaitForSeconds(0.5f);
        }
    }
}
