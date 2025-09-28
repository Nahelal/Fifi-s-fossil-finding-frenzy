using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialButton : MonoBehaviour
{
    [SerializeField] GameObject hubTutorial;
    [SerializeField] GameObject diggingTutorial;
    [SerializeField] GameObject BuildingTutorial;

    private void Update()
    {
        //potential fix for stuff below??
        if (Input.anyKey)
        {
            hubTutorial.SetActive(false);
            diggingTutorial.SetActive(false);
            BuildingTutorial.SetActive(false);
        }
    }

    public void ToggleTutorialPopup()
    { 
        string activeSceneName = SceneManager.GetActiveScene().name;

        //potentiual bug if the player transitions scenes with the tutorial open
        //would be good to turn off inputs while its open??? how?
        switch (activeSceneName)
        {
            case "Outside Level Design":
                hubTutorial.SetActive(!hubTutorial.activeInHierarchy);
                break;
            case "Museum":
                BuildingTutorial.SetActive(!BuildingTutorial.activeInHierarchy);
                break;
            case "DiggingMinigame":
                diggingTutorial.SetActive(!diggingTutorial.activeInHierarchy);
                break;
            case "BoneConstruction2D":
                BuildingTutorial.SetActive(!BuildingTutorial.activeInHierarchy);
                break;
            default:
                break;
        }
    }
}
