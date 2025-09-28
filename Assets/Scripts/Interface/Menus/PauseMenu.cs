using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool bIsPaused;

    public string MenuScene;
    public GameObject pauseScreen;

    // Update is called once per frame
    void Update()
    {
        if (ConversationUI.IsRunning()) return;
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //flip between t/f
            bIsPaused = !bIsPaused;
            if (bIsPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;

        pauseScreen.gameObject.SetActive(true);
    }
    public void ResumeGame()
    {
        //in case of button
        bIsPaused = false;

        Time.timeScale = 1.0f;
        pauseScreen.gameObject.SetActive(false);
    }

    public void GoToMainMenu()
    {
        Debug.Log(bIsPaused);

        SceneManager.LoadScene(MenuScene);
        Debug.Log("chanigng scene");
    }

}
