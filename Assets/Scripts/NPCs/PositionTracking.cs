using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PositionTracking : MonoBehaviour
{
    [SerializeField] string trackingID;
    [SerializeField] bool targetHasCharacterControllerComponent;
    [SerializeField] bool targetHasNavMeshAgentComponent;
    [Tooltip("Toggle On when you don't want to use the last saved position on Enable, remember to turn it off!")]
    [SerializeField] bool emergencySwitch;

    // key for testing

    private void Awake()
    {
        if (emergencySwitch)
        {
            SavePosition();
        }
    }

    public void SavePosition()
    {
        PlayerPrefs.SetFloat(trackingID + "X" + SceneManager.GetActiveScene().name, transform.position.x);
        PlayerPrefs.SetFloat(trackingID + "Y" + SceneManager.GetActiveScene().name, transform.position.y);
        PlayerPrefs.SetFloat(trackingID + "Z" + SceneManager.GetActiveScene().name, transform.position.z);
    }

    public void UpdatePlayerPosition()
    {
        if (!PlayerPrefs.HasKey(trackingID + "X" + SceneManager.GetActiveScene().name))
        {
            return;
        }

        Vector3 savedPos = new Vector3
            (PlayerPrefs.GetFloat(trackingID + "X" + SceneManager.GetActiveScene().name
            ), PlayerPrefs.GetFloat(trackingID + "Y" + SceneManager.GetActiveScene().name
            ), PlayerPrefs.GetFloat(trackingID + "Z" + SceneManager.GetActiveScene().name));

        if (targetHasCharacterControllerComponent)
        {
            GetComponent<CharacterController>().enabled = false;
            transform.position = savedPos;
            GetComponent<CharacterController>().enabled = true;
        }
        else if (targetHasNavMeshAgentComponent)
        {
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = savedPos;
            GetComponent<NavMeshAgent>().enabled = true;
        }
        else
        {
            transform.position = savedPos;
        }

    }

    private void OnDisable()
    {
        SavePosition();
    }

    private void OnEnable()
    {
        UpdatePlayerPosition();
    }
}
