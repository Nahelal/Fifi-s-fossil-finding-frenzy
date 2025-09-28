// SceneStateAutoSaver2023.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveState : MonoBehaviour
{

    public static SaveState Instance;
    readonly Dictionary<string, string> _cache = new();


    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void Save()
    {
        var scene = SceneManager.GetActiveScene();
        _cache[scene.path] = SaveInventoryItems.StoreInventories(scene);
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        StartCoroutine(RestoreNextFrame(scene));
    }

    IEnumerator RestoreNextFrame(Scene scene)
    {
        yield return null;  // let every component finish its own Start()
        if (_cache.TryGetValue(scene.path, out var json))
        {
            SaveInventoryItems.RestoreInventories(json);
        }
    }
}
