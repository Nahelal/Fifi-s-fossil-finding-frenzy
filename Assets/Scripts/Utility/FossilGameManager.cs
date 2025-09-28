using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FossilGameManager : MonoBehaviour
{
    public static FossilGameManager instance;

    //public Transform selectedPodium;
    public string selectedPodiumName;

    public GameObject combinedBoneObject;
    private GameObject fossilRoot;

    public Dictionary<string, Vector3> characterPositions = new Dictionary<string, Vector3>();

    private Dictionary<string, GameObject> podiumFossils = new Dictionary<string, GameObject>();

    //keep alive between scene changes
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("manager created");
            fossilRoot = new GameObject("FossilRoot");
            DontDestroyOnLoad(fossilRoot);
        }
        else
        {
            Debug.Log("duplicate manager destroyed");
            Destroy(gameObject);
            return;
        }
    }

    //! player positions
    public void SaveCharacterPosition(string characterID, Vector3 position)
    {
        characterPositions[characterID] = position;
    }

    public Vector3? GetCharacterPosition(string characterID)
    {
        if (characterPositions.TryGetValue(characterID, out Vector3 pos))
        {
            return pos;
        }
        return null;
    }

    //grab the text object

    //! Podium
    public void SetSelectedPodium(Transform podium)
    {
        //name of podium 
        selectedPodiumName = podium.name;
        Debug.Log(selectedPodiumName);
    }
    public string GetSelectedPodium() => selectedPodiumName;

    //! bones
    public void SetCombinedBones(string podiumName, GameObject combinedBones)
    {
        if (!string.IsNullOrEmpty(podiumName) && combinedBones != null)
        {
            combinedBones.transform.SetParent(fossilRoot.transform);

            combinedBones.SetActive(false);
            DontDestroyOnLoad(combinedBones);

            podiumFossils[podiumName] = combinedBones;
        }
    }

    public Dictionary<string, GameObject> GetAllFossils()
    {
        return podiumFossils;
    }
}
