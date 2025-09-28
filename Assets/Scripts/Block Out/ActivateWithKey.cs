using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWithKey : MonoBehaviour
{
    public GameObject[] activate;
    public GameObject[] deactivate;
    public KeyCode key;

    public FossilFinder proximityAudioScript;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key) && proximityAudioScript.closestDigSite != null)
        {
            DigUpSite(proximityAudioScript.closestDigSite.GetComponent<Digsite>());

            foreach (var go in activate)
            {
                go.SetActive(true);
            }
            foreach (var go in deactivate)
            {
                go.SetActive(false);
            }
        }
    }

    void DigUpSite(Digsite t)
    {
        if (proximityAudioScript.digsites.Contains(t))
        {
            //remove site location from list 
            proximityAudioScript.digsites.Remove(t);
            
            Debug.Log("dig site removed from list yay");
        }
    }
}
