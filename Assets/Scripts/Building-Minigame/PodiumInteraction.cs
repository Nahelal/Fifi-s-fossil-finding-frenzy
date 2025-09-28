using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PodiumInteraction : MonoBehaviour
{

    public static PodiumInteraction Instance;
    public static Transform selectedPodium;
    private bool bAlreadyBuilt = false;

    public GameObject namePanel;
    public TMPro.TMP_Text fossilNameText;
    public bool bIsActive;

    public GameObject notEnoughBones;

    public GameObject interactionCollider;

    public ChangeScene sceneChanger;


    private void Start()
    {
        notEnoughBones.SetActive(false);
    }

    public void EnoughBonesCheck()
    {
        if (bAlreadyBuilt)
        {
            return;
        }

        string podiumName = transform.name;

        //cant build if if already made fossil here
        if (FossilGameManager.instance.GetAllFossils().ContainsKey(podiumName))
        {
            ShowDinoNameUI();
            return;
        }

        PodiumRequirements requirements = GetComponent<PodiumRequirements>();
        if (requirements != null && requirements.bCanBuild())
        {
            selectedPodium = transform;
            FossilGameManager.instance.SetSelectedPodium(transform);

            requirements.SelectBones();
            Debug.Log(fossilNameText.gameObject.name);
            bAlreadyBuilt = true;

            if (sceneChanger != null)
            {
                sceneChanger.TriggerSceneChange();
            }
        }
        else
        {
            if (notEnoughBones != null)
            {
                notEnoughBones.SetActive(true);
            }
        }

    }


    private void OnTriggerExit(Collider other)
    {
        notEnoughBones.SetActive(false);

        if (bIsActive == true)
        {
            bIsActive = false;
            namePanel.SetActive(false);
        }
    }

    public void ShowDinoNameUI()
    {
        Debug.Log("slay");
        bIsActive = !bIsActive;
        namePanel.SetActive(bIsActive);

        if (BoneManager.instance.TryGetFossilName(transform.name, out string fossilName))
        {
            fossilNameText.text = fossilName;
        }
    }

}


