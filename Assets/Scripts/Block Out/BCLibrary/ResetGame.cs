using UnityEngine;


[DefaultExecutionOrder(-20000)]
public class ResetGame : MonoBehaviour
{
    private static ResetGame _instance;

    void Awake()
    {
        if (_instance && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save(); // Optional: force the changes to be written immediately
        Debug.Log("All PlayerPrefs have been cleared.");

    }
}