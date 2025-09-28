using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyself : MonoBehaviour
{

    // Start is called before the first frame update
    private IEnumerator destroy_later()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject.Destroy(this.gameObject);
    }


    void Awake()
    {
        StartCoroutine(destroy_later());
    }

}
