using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneManager : MonoBehaviour
{
    public static BoneManager instance;
    [HideInInspector] public PersistentBoneUI boneUiInstance;

    public int totalBonesCollected;
    public List<GameObject> selectedBuildBones = new List<GameObject>();

    public Dictionary<string, string> fossilNames = new Dictionary<string, string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        totalBonesCollected = PlayerPrefs.GetInt("collected_bones");
        boneUiInstance = FindAnyObjectByType<PersistentBoneUI>();

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
           PersistentBoneUI.Instance.AppendBoneToCounter(10);
        }
    }

    //for the digging minigame
    public void AddBone(int input = 1)
    {
        totalBonesCollected = input;
        Debug.Log(":" + totalBonesCollected);

    }

    //podium checks bones for building minigame
    public bool bHasEnoughBones(int requiredBones)
    {
        return totalBonesCollected >= requiredBones;
    }

    //remove bones on build
    public void ConsumeBones(int amount)
    {
        //  totalBonesCollected -= amount;
        if (amount > totalBonesCollected)
        {
            return;
        }

        boneUiInstance.AppendBoneToCounter(-amount);
    }

    public void SetSelectedBuildBones(List<GameObject> bones)
    {
        selectedBuildBones = bones;
    }

    //ref when moving to building minigame scene for which bones to put on sprite points
    public List<GameObject> GetSelectedBuildBones()
    {
        return selectedBuildBones;
    }

    public void SetFossilName(string podiumName, string fossilName)
    {
        fossilNames[podiumName] = fossilName;
    }

    public bool TryGetFossilName(string podiumName, out string fossilName)
    {
        return fossilNames.TryGetValue(podiumName, out fossilName);
    }

    public bool HasFossilAtPodium(string podiumName)
    {
        return fossilNames.ContainsKey(podiumName);
    }
}
