using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoneSpawning : MonoBehaviour
{

    //bone spawn 
    public Transform[] boneSpawnLocations;
    public GameObject[] selectedBones;

    private int totalBones;


    void OnEnable()
    {
        //Debug.Log("BoneSpawning OnEnable!");
        SpawnBones();
    }

    private void SpawnBones()
    {
        //bones being spawned
        List<GameObject> selectedBones = BoneManager.instance.GetSelectedBuildBones();
        totalBones = selectedBones.Count;

        for (int i = 0; i < selectedBones.Count && i < boneSpawnLocations.Length; i++)
        {
            GameObject bone = Instantiate(selectedBones[i], boneSpawnLocations[i].position, Quaternion.identity);
            FossilConstruction fossilScript = bone.GetComponent<FossilConstruction>();
            bone.SetActive(true);

            if (fossilScript != null)
            {
                fossilScript.spawnManager = this;
            }
        }
    }



    public int GetTotalBones()
    {
        return totalBones;
    }
}
