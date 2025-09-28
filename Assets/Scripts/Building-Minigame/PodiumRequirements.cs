using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PodiumRequirements : MonoBehaviour
{
    private const string T = "BonesNeeded: ";

    //total bones needed for building minigame to start
    public int totalBonesRequired = 5;

    //public GameObject UIPrefab;
    public TextMeshProUGUI bonesText;


    //pools of bones
    public GameObject[] headBones;
    public GameObject[] torsoBones;
    public GameObject[] limbBones;
    public GameObject[] decorBones;
    public GameObject[] bonusBone;

    //bone types needed for this podium
    public int headsNeeded = 1;
    public int torsosNeeded = 1;
    public int limbsNeeded = 2;
    public int decorsNeeded = 1;
    public int bonusNeeded = 0;

    public List<GameObject> selectedBones = new List<GameObject>();

    private void Awake()
    {
        string podiumName = gameObject.name;

        if (BoneManager.instance.HasFossilAtPodium(podiumName))
        {
            bonesText.gameObject.SetActive(false); 
        }
        else
        {
            UpdateBoneUI(); 
        }
    }

    public bool bCanBuild()
    {
        //check if player has enough to enter building minigame
        return BoneManager.instance.bHasEnoughBones(totalBonesRequired);
    }

    public void SelectBones()
    {
        Debug.Log("selecting bones...");
        bonesText.text = "";
        selectedBones.Clear();

        //random boner
        AddRandomBones(headsNeeded, headBones);
        AddRandomBones(torsosNeeded, torsoBones);
        AddRandomBones(limbsNeeded, limbBones);
        AddRandomBones(decorsNeeded, decorBones);
        AddRandomBones(bonusNeeded, bonusBone);

        //use up bones from the total in bone manager
        BoneManager.instance.SetSelectedBuildBones(selectedBones);
        BoneManager.instance.ConsumeBones(totalBonesRequired);


    }
    private void AddRandomBones(int count, GameObject[] pool)
    {
        for (int i = 0; i < count; i++)
        {
            var bone = GetRandom(pool);
            if (bone != null)
            {
                selectedBones.Add(bone);
            }
        }
    }

    //get random object in the array 
    private GameObject GetRandom(GameObject[] pool)
    {
        if (pool.Length == 0)
        {
            return null;
        }

        return pool[Random.Range(0, pool.Length)];

    }

    void UpdateBoneUI()
    {
        //boner text
        bonesText.text = "<sprite=0>= " + totalBonesRequired;
    }
}
