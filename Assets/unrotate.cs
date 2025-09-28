using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unrotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame

}
