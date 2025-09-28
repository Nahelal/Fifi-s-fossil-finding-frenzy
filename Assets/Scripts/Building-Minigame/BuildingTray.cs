using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTray : MonoBehaviour
{
    public BoneSpawning boneSpawn;
    public GameObject buildButton;

    private HashSet<GameObject> bonesInTray = new HashSet<GameObject>();

    // Start is called before the first frame update
    //button now triggered by bone location within tray rather than the joints... fiddling after crit for suresies
    void Start()
    {
        buildButton.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bone"))
        {
            Debug.Log("bones entered");
            bonesInTray.Add(other.gameObject);
            CheckBonesInTray();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bone"))
        {  
            Debug.Log("bones left");
            bonesInTray.Remove(other.gameObject);
            CheckBonesInTray();
        }
    }

    public bool bBoneInTray(GameObject bone)
    {
        return bonesInTray.Contains(bone);
    }

    void CheckBonesInTray()
    {
        int totalBones = boneSpawn.GetTotalBones();

        if (bonesInTray.Count >= totalBones)
        {
            buildButton.SetActive(true);
        }
        else
        {
            //Debug.Log(bonesInTray.Count);
            buildButton.SetActive(false);
        }

    }
}
