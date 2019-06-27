using System.Collections;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private string _label = "";
    private float _count;
    private Color _color = Color.green;
    [SerializeField]
    private GUIStyle _style = default;
 
    private IEnumerator Start()
    {
        GUI.depth = 2;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            _count = (1 / Time.deltaTime);
            _label = "FPS: " + (Mathf.Round(_count));
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 100, 25), _label, _style);
    }
}
