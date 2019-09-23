using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Prefab attribute. Use this on child classes
/// to define if they have a prefab associated or not
/// By default will attempt to load a prefab
/// that has the same name as the class,
/// otherwise [Prefab("path/to/prefab")]
/// to define it specifically. 
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class PrefabAttribute : Attribute
{
	string _name;
	public string Name { get { return this._name; } }
	public PrefabAttribute() { this._name = ""; }
	public PrefabAttribute(string name) { this._name = name; }
}

/// <summary>
/// Use this class to create a singleton from a given prefab.
/// To make it persistant between scenes, addd the component PersistantBetweenScene on the prefab.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class PrefabSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Check to see if we're about to be destroyed.
    private static bool _shuttingDown = false;
    private static object _lock = new object();
    private static T _instance;

    /// <summary>
    /// Access singleton instance through this propriety.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_shuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed. Returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    // Search for existing instance.
                    _instance = (T)FindObjectOfType(typeof(T));

                    // Create new instance if one doesn't already exist.
                    if (_instance == null)
                    {
                        // We load the corresponding prefabs
                        bool isPrefabPathDefined = Attribute.IsDefined(typeof(T), typeof(PrefabAttribute));
                        if (!isPrefabPathDefined)
                        {
                            Debug.LogError("Prefab path is not defined in prefab attribute.");
                            return null;
                        }
                        PrefabAttribute attr = (PrefabAttribute) Attribute.GetCustomAttribute(typeof(T), typeof(PrefabAttribute));
                        string prefabPath = attr.Name;
                        var prefab = Utils.GetPrefab(prefabPath);
                        var singletonObject = Instantiate(prefab);
                        _instance = singletonObject.GetComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";
                    }
                }
                return _instance;
            }
        }
    }


    private void OnApplicationQuit()
    {
        _shuttingDown = true;
    }


    private void OnDestroy()
    {
        _shuttingDown = true;
    }
}
