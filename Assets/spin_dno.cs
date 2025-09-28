using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin_dno : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3[] vecs;

    private Vector3 rot;

     void Update()
     {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(rotate_slowly(vecs[Random.Range(0, vecs.Length)], new GameObject()));
        }
     }




        private IEnumerator rotate_slowly(Vector3 rot, GameObject targ)
        {
        float timeT = 0f;
        do
        {
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(rot), timeT);
            timeT += Time.deltaTime;    
            yield return null;

        } while (gameObject.transform.rotation != Quaternion.Euler(rot));
     
    }
}
