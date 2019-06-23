using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to the game UI
public class GameUIManager : MonoBehaviour
{
    private static bool _alreadyExists;

    private void Awake()
    {
        if (!_alreadyExists)
        {
            _alreadyExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
