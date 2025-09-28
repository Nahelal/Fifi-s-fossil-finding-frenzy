using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FossilAssemble : MonoBehaviour
{
    //new mesh to be made 
    public GameObject combinedBoneObject;

    public GameObject namingUIPanel;
    public GameObject backingPanel;
    public TMPro.TMP_InputField nameInput;
    public string playerInput = "fart boy";
    //[SerializeField] Material spriteShadowMaterial;

    public void SetName()
    {
        //call on button in builing 
        namingUIPanel.SetActive(true);
        backingPanel.SetActive(true);
    }

    public void ConfirmDinoName()
    {
        Debug.Log("confirm name func called");
        playerInput = nameInput.text;

        if (string.IsNullOrEmpty(playerInput))
        {
            Debug.LogWarning("cant be emoty boy");
            return;
        }

        string selectedPodium = FossilGameManager.instance.GetSelectedPodium();
        string podiumName = selectedPodium;

        BoneManager.instance.SetFossilName(podiumName, playerInput);

        Debug.Log(playerInput);
        Debug.Log(podiumName);
        Debug.Log("confirm name func finished!");

        FindObjectOfType<ChangeScene>().TriggerSceneChange("Museum");

    }


    public void AssembleBones()
    {
        //create empty game object
        if (combinedBoneObject == null)
        {
            combinedBoneObject = new GameObject("CombinedBones");
        }

        //gathweing sprites for combined object in new game objct 
        List<SpriteRenderer> connectedBoneSprites = new List<SpriteRenderer>();

        foreach (GameObject bone in GameObject.FindGameObjectsWithTag("Bone"))
        {
            SpriteRenderer spriteRenderer = bone.GetComponent<SpriteRenderer>();
            //adds mesh data 
            if (spriteRenderer != null && bIsBoneConnected(bone))
            {
                connectedBoneSprites.Add(spriteRenderer);
            }
        }

        //zero bones connected 
        if (connectedBoneSprites.Count == 0)
        {
            Debug.Log("no bones to connect");

            return;
        }

        //faking combine connected bones under same gameobject yay
        foreach (var renderer in connectedBoneSprites)
        {
            //new object under new parent to conbine sprites
            GameObject spriteObject = new GameObject(renderer.gameObject.name);
            spriteObject.transform.SetParent(combinedBoneObject.transform, false);
            //keep local transform so glooping isnt happening 
            spriteObject.transform.localPosition = renderer.transform.localPosition;
            spriteObject.transform.localRotation = renderer.transform.localRotation;
            spriteObject.transform.localScale = renderer.transform.localScale;

            //copying spriterenderer from the individial bones
            SpriteRenderer newRenderer = spriteObject.AddComponent<SpriteRenderer>();
            newRenderer.sprite = renderer.sprite;
            newRenderer.sortingLayerID = renderer.sortingLayerID; 
            newRenderer.sortingOrder = renderer.sortingOrder;

            newRenderer.material = renderer.material;
            newRenderer.shadowCastingMode = renderer.shadowCastingMode;
            newRenderer.receiveShadows = renderer.receiveShadows;
        }

        //keep the bones aliveeee
        FossilGameManager.instance.SetCombinedBones(FossilGameManager.instance.selectedPodiumName, combinedBoneObject);

        DontDestroyOnLoad(combinedBoneObject);
    }


    private bool bIsBoneConnected(GameObject bone)
    {
        foreach (Transform joint in bone.transform)
        {
            if (joint.CompareTag("BoneJoint"))
            {
                return true;
            }
        }
        return false;

    }

    //todo: add key popup for interaction when close!
    //todo: add visuals for bone counter

    //todo: refactor literally all my code
    //todo: podium check for interaction
    //todo: when the bones are detached, move back to spawn location 

    //todo: add proximity audio back for player

    //todo: only let bones connect when they are in the bone tray!


}
