using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class create_particle : MonoBehaviour
{
    public GameObject particle;
    public GameObject pebble_particle;
    // Start is called before the first frame update
    void OnDestroy()
    {
        GameObject.Instantiate(particle, transform.position, Quaternion.identity);
    }



    public void create_peb(Transform trans)
    {

        GameObject.Instantiate(pebble_particle, trans.position, Quaternion.identity);

    }

}
