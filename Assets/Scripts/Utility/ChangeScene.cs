using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    //todo: make work for the digging and building minigame with the scene transitions
    //todo: check player pos works for going into the building etc 

    public string SceneName;
    public Image cutoutRenderer;
    public Image backgroundRenderer;
    public float fadeLength = 2.0f;

    //private bool bPlayerInRange = false;

    private void Awake()
    {
        cutoutRenderer = GameObject.FindGameObjectWithTag("SceneTransitionCutout").GetComponent<Image>();
        backgroundRenderer = GameObject.FindGameObjectWithTag("SceneTransitionBackground").GetComponent<Image>();

        ChangeTransitionByScene(SceneManager.GetActiveScene().name);
    }
    public void TriggerSceneChange(string sceneToLoad = "Outside Level Design")
    {
        //null check bc errors
        if (SaveState.Instance != null)
        {
            SaveState.Instance.Save();
        }

        sceneToLoad = (sceneToLoad != null) ? sceneToLoad = SceneName : sceneToLoad;

        Animator fadeanim = cutoutRenderer.GetComponent<Animator>();

        if (fadeanim != null)
        {
            Debug.Log("animation playing");
            fadeanim.SetTrigger("LoadOut");
        }

        if (sceneToLoad == "DiggingMinigame")
        {
            LoadNextPuzzle();
        }
        else StartCoroutine(LoadSceneOnInteract(sceneToLoad));
    }

    public void LoadNextPuzzle()
    {
        if (!PlayerPrefs.HasKey("level_to_load"))
        {
            PlayerPrefs.SetInt("level_to_load", 0);
        }
        StartCoroutine(LoadSceneOnInteract("DiggingMinigame"));
    }

    //scene change delay
    IEnumerator LoadSceneOnInteract(string input)
    {
        ChangeTransitionByScene(input);

        yield return new WaitForSeconds(fadeLength);

        SceneManager.LoadSceneAsync(input);
    }
    //make sure to disable movement for the player when scene transitioning

    void ChangeTransitionByScene(string sceneToTransitionTo)
    {
        switch (sceneToTransitionTo)
        {
            case "Outside Level Design":
                cutoutRenderer.sprite = cutoutRenderer.GetComponent<SceneTransitionCutoutHolder>().outsideLevelDesignCutout;
                backgroundRenderer.sprite = backgroundRenderer.GetComponent<SceneTransitionBackgroundHolder>().outsideLevelDesignBackground;
                backgroundRenderer.color = backgroundRenderer.GetComponent<SceneTransitionBackgroundHolder>().outsideLevelDesignColor;
                break;
            case "Museum":
                cutoutRenderer.sprite = cutoutRenderer.GetComponent<SceneTransitionCutoutHolder>().museumCutout;
                backgroundRenderer.sprite = backgroundRenderer.GetComponent<SceneTransitionBackgroundHolder>().museumBackground;
                backgroundRenderer.color = backgroundRenderer.GetComponent<SceneTransitionBackgroundHolder>().museumColor;
                break;
            case "DiggingMinigame":
                cutoutRenderer.sprite = cutoutRenderer.GetComponent<SceneTransitionCutoutHolder>().diggingCutout;
                backgroundRenderer.sprite = backgroundRenderer.GetComponent<SceneTransitionBackgroundHolder>().diggingBackground;
                backgroundRenderer.color = backgroundRenderer.GetComponent<SceneTransitionBackgroundHolder>().diggingColor;
                break;
            case "BoneConstruction2D":
                cutoutRenderer.sprite = cutoutRenderer.GetComponent<SceneTransitionCutoutHolder>().boneBuildingCutout;
                backgroundRenderer.sprite = backgroundRenderer.GetComponent<SceneTransitionBackgroundHolder>().boneBuildingBackground;
                backgroundRenderer.color = backgroundRenderer.GetComponent<SceneTransitionBackgroundHolder>().boneBuildingColor;
                break;
            default:
                break;
        }
    }
}
