using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSprite : MonoBehaviour
{
    [SerializeField] SpriteRenderer targetRenderer;

    [SerializeField] bool doFade;
    [SerializeField] float targetAlpha;
    [SerializeField] float fadedAlpha;
    [SerializeField] float originalAlpha;
    [SerializeField] float lerpSpeed;

    private void Awake()
    {
        originalAlpha = targetRenderer.material.GetFloat("_Alpha_Multiplier");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CameraFadeTrigger"))
        {
            doFade = true;
            targetAlpha = fadedAlpha;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CameraFadeTrigger"))
        {
            doFade = false;
            targetAlpha = originalAlpha;
        }
    }

    private void Update()
    {
        //targetRenderer.material.SetFloat("_Alpha_Multiplier", Mathf.Lerp(targetRenderer.material.GetFloat("_Alpha_Multiplier"), targetAlpha, Time.deltaTime * lerpSpeed));

        if (doFade)
        {
            targetRenderer.material.SetFloat("_Alpha_Multiplier", Mathf.Lerp(targetRenderer.material.GetFloat("_Alpha_Multiplier"), targetAlpha, Time.deltaTime * lerpSpeed));
        }
        else
        {
            targetRenderer.material.SetFloat("_Alpha_Multiplier", Mathf.Lerp(targetRenderer.material.GetFloat("_Alpha_Multiplier"), originalAlpha, Time.deltaTime * lerpSpeed));
        }
    }

    /*SpriteRenderer mySpriteRenderer;

    [SerializeField] float fadeTransparency;
    [SerializeField] float defaultTransparency;
    [SerializeField] float fadeSpeed;
    [SerializeField] float test;

    float timer;
    [SerializeField] float fadeDuration;

    private void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("object foundddd");
        if (other.CompareTag("Obscurer"))
        {
            doFade = true;
            Debug.Log("lmaoooo ay");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obscurer"))
        {
            doFade = false; 
        }
    }

    private void Update()
    {
        var currentColor = mySpriteRenderer.material.color;

        if (doFade)
        {
            currentColor.a = Mathf.MoveTowards(currentColor.a, fadeTransparency, Time.deltaTime * fadeSpeed);
            Debug.Log(currentColor.a);
        }
        else
        {
            
            currentColor.a = Mathf.MoveTowards(currentColor.a, fadeTransparency, Time.deltaTime * -fadeSpeed);
        }

        mySpriteRenderer.material.SetFloat("_Alpha_Mulitplier", currentColor.a);
    }*/
}
