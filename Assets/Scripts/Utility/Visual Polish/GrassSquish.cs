using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GrassSquish : MonoBehaviour
{
    [SerializeField] Transform scalingTransform;

    Vector3 originalScale;
    [SerializeField] float scaleFactor = 10f;

    bool isSquish;
    Vector3 scaleTarget;
    [SerializeField] float lerpSpeed;


    private void Start()
    {
        originalScale = scalingTransform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        //originalColliderRadius = myCollider.radius;

        if (other.CompareTag("Player"))
        {
            isSquish = true;
            //Squish
            scaleTarget = new Vector3(originalScale.x, originalScale.y - originalScale.y / scaleFactor, originalScale.z);
            //myCollider.radius = myCollider.radius - myCollider.radius / scaleFactor;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isSquish = false;
            //AntiSquish
            scaleTarget = new Vector3(originalScale.x, originalScale.y + originalScale.y / scaleFactor, originalScale.z);
           // myCollider.radius = myCollider.radius + myCollider.radius / scaleFactor;
        }
    }

    private void Update()
    {
        if (isSquish)
        {
            scalingTransform.localScale = Vector3.MoveTowards(scalingTransform.localScale, scaleTarget, Time.deltaTime * lerpSpeed);
        }
        else
        {
            scalingTransform.localScale = Vector3.MoveTowards(scalingTransform.localScale, originalScale, Time.deltaTime * lerpSpeed);
        }
    }
}
