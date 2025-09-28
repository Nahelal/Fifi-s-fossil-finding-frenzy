using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class FoliageTransformRandomiser : MonoBehaviour
{
    [SerializeField] bool randomiseScale;
    [SerializeField] float widthVariance;
    [SerializeField] float heightVariance;

    [SerializeField] bool randomiseYRotation;
    [SerializeField] float rotationVariance;

    [SerializeField] bool randomiseYPosition;
    [SerializeField] float positionVariance;

    void Start()
    {
        //Flips sprites
        RandomiseX();

        if (randomiseScale) RandomiseWidthHeight();
        if (randomiseYRotation) RandomiseYRotation();
        if (randomiseYPosition) RandomiseYPosition();
    }

    void RandomiseWidthHeight()
    {
        Vector3 newDimensions = transform.localScale;

        newDimensions.x += Random.Range(-widthVariance, widthVariance);
        newDimensions.y += Random.Range(-heightVariance, heightVariance);

        transform.localScale = newDimensions;
    }

    void RandomiseX()
    {
        if (Random.Range(0, 2) == 0) 
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void RandomiseYRotation()
    {
        Vector3 newRotation = transform.eulerAngles;

        newRotation.y += Random.Range(-rotationVariance, rotationVariance);

        transform.eulerAngles = newRotation;
    }

    void RandomiseYPosition()
    {
        Vector3 newPosition = transform.position;

        newPosition.y += Random.Range(-positionVariance, positionVariance);

        transform.position = newPosition;
    }
}
