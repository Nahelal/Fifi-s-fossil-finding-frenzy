using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class r : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("DiggingMinigame");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.SetInt("level_to_load", 0);
        }
    }
}
