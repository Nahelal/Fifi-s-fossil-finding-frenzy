using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOpeningTrans : MonoBehaviour
{
    public Animator fadeObj;
    public float fadeLength = 2.0f;


    void Awake()
    {
        fadeObj = GetComponent<Animator>();

        if (fadeObj != null)
        {
            Debug.Log("animation playing");
            fadeObj.SetTrigger("LoadIn");
        }

        StartCoroutine(LoadSceneOnInteract());
    }

    //scene change delay
    IEnumerator LoadSceneOnInteract()
    {
        yield return new WaitForSeconds(fadeLength);
        //destroy after scene transition is compelte
        Destroy(gameObject);

        //todo:pause player movement until this transition is complete also!
    }
}
