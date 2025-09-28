using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Digsite : MonoBehaviour
{
    FossilFinder fossilFinder;
    bool isFound;
    [SerializeField] GameObject digsiteRemains;

    public Interact interact;

    private void Awake()
    {
        interact = GetComponent<Interact>();
        //ref to dinomite's fossil finding script
        fossilFinder = FindFirstObjectByType<FossilFinder>();

        if (PlayerPrefs.HasKey(gameObject.name))
        {
            Debug.Log(gameObject.name + " found");
            //if dug out already
            if (PlayerPrefs.GetInt(gameObject.name) == 1)
            {
                //Debug.Log(gameObject.name + " has been dug out already");
                isFound = true;
                fossilFinder.digsites.Remove(this);

                digsiteRemains.SetActive(true);
            }
        }
        else
        {

            PlayerPrefs.SetInt(gameObject.name, 0);
            //Debug.Log(gameObject.gameObject.name + " now set in PlayerPrefs");
        }
    }

    public void OnInteract()
    {
        if (PlayerPrefs.GetInt(gameObject.name) == 1)
        {
            //Debug.Log("i've dug here already...");

            //player comments on it
        }
        else
        {
            //Debug.Log("digsite found!");

            isFound = true;
            PlayerPrefs.SetInt(gameObject.name, 1);

            //pet animation


            //load scene
            ChangeScene changeScene = GetComponent<ChangeScene>();
            changeScene.TriggerSceneChange("DiggingMinigame");

        }
    }
}
