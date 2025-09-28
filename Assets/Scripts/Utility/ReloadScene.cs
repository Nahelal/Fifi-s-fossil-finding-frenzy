using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    [SerializeField] KeyCode ReloadKey;

    void Update()
    {
        if(Input.GetKeyDown(ReloadKey)) {SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    }
}
