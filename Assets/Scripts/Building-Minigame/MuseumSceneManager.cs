using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumSceneManager : MonoBehaviour
{
    public FossilGameManager gameManager;

    //todo make sure fossils go to their designated podiums when scene load 

    private void Awake()
    {
        PlaceFossilsOnPodiums();
    }

    void PlaceFossilsOnPodiums()
    {
        var fossils = FossilGameManager.instance.GetAllFossils();

        foreach (var podiumFossil in fossils)
        {
            string podiumName = podiumFossil.Key;
            GameObject fossil = podiumFossil.Value;

            //find pod
            GameObject podiumObj = GameObject.Find(podiumName);
            if (podiumObj == null || fossil == null)
            {
                continue;
            }

            //find fossil slot chukd
            Transform fossilSlot = podiumObj.transform.Find("FossilSlot");
            if (fossilSlot == null)
            {
                continue;
            }

            //clone of the bones for the podiums + can see 
            GameObject fossilInstance = Instantiate(fossil, fossilSlot);
            fossilInstance.SetActive(true);

            fossilInstance.transform.SetParent(fossilSlot);
            fossilInstance.transform.localPosition = Vector3.zero;
            fossilInstance.transform.localScale = Vector3.one;
        }
    }
}
