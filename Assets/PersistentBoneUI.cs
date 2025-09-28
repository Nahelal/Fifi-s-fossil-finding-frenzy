using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

[DefaultExecutionOrder(10000)]
public class PersistentBoneUI : MonoBehaviour
{
    BoneManager boneManager;
    [SerializeField] TextMeshProUGUI boneCounterText;
    public static PersistentBoneUI Instance { get; private set; }

    //If the player wnet back to the main menu I would assume this object would still follow, which could be an issue.
    //The jank fix is to make sure this canvas renders behind the main menu so that you couldn't see it.

    private void Awake()
    {
        boneCounterText.text = "<sprite=0>x" + PlayerPrefs.GetInt("collected_bones");
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }

        //UpdateBoneCounter(10);

        //pull this out of any other objects so that it scales properly
        transform.parent = null;

        if (BoneManager.instance)
        {
            BoneManager.instance.boneUiInstance = Instance;
        }

    }


    public void AppendBoneToCounter(int bones)
    {
        var arr = boneCounterText.text.Split("x");
        bones += Convert.ToInt32(arr[1]);
        boneCounterText.text = arr[0] + "x" + bones;
        PlayerPrefs.SetInt("collected_bones", bones);
        BoneManager.instance.AddBone(bones);
    }


}
