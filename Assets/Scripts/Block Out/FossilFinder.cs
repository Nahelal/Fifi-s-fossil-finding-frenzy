using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

[DefaultExecutionOrder(-100)]
public class FossilFinder : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Material dinomiteTailMaterial;

    [Header("Fossil Finding Settings")]
    [HideInInspector] public List<Digsite> digsites = new();
    public float range;

    private float currentBeepDelay;
    private float lastBeep;

    public Transform closestDigSite;

    [SerializeField] private bool bIsBeeping = false;

    [Header("Beeping Settings")]

    public float minBeepDelay; //close
    public float maxBeepDelay; //far
    //public AudioSource beepAudio; <-- this method is defo more efficient but im just keeping all audio in the same SO system for now
    public GameObject beepSoundObject;
    [SerializeField] GameObject toggleOnSO;
    [SerializeField] GameObject toggleOffSO;
    [SerializeField] float minPitch;
    [SerializeField] float maxPitch;
    [SerializeField] float volume;
    [SerializeField] float lightOnTime;
    [SerializeField] MeshRenderer MeshRendererToUpdate;
    [SerializeField] Material lightOnMaterial;
    [SerializeField] Material lightOffMaterial;  

    private void Awake()
    {
        //find digsites by script
        //will have to make sure dug out digsites don't get added / get removed after this is called
        digsites.AddRange(FindObjectsByType<Digsite>(FindObjectsSortMode.InstanceID));
    }

    void Update()
    {
        //B input for beep... move this after pt to characterconteoller
        if (Input.GetKeyDown(KeyCode.B))
        {
            //toggle beep on keypress
            bIsBeeping = !bIsBeeping;
            if (bIsBeeping)
            {
                Instantiate(toggleOnSO);
            }
            else
            {
                Instantiate(toggleOffSO);
            }
        }
        foreach (Digsite d in digsites)
        {
            d.interact.active = false;
        }

        if (bIsBeeping)
        {
            FindClosestDigSite();

            if (closestDigSite != null)
            {
                //distance between site and player
                float distance = Vector3.Distance(transform.position, closestDigSite.position);
                UpdateBeepRate(distance);
            }
            else
            {
                //maintain slow beep if no sites near
                currentBeepDelay = maxBeepDelay;
            }

            //play beeps 
            if (Time.time - lastBeep >= currentBeepDelay)
            {
                //beepAudio.Play();
                float distance = Vector3.Distance(transform.position, closestDigSite.position);
                Beep(distance);


                //add blinking light here
                StartCoroutine(BlinkingLight());

                lastBeep = Time.time;

            }
        }
        else
        {
            //noo beeping
            //beepAudio.Stop();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("PlayerPrefs Deleted");
        }
    }

    void FindClosestDigSite()
    {
        //min dist set to infinity so first site will always be closest 
        float minDistance = Mathf.Infinity;
        closestDigSite = null;

        foreach (Digsite t in digsites)
        {
            //checks if site is closer than previous 
            float distance = Vector3.Distance(transform.position, t.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                t.interact.active = true;
                //update closest site 
                closestDigSite = t.transform;
            }
        }
    }

    void UpdateBeepRate(float distance)
    {
        float rangeSqr = range * range;

        if (distance <= rangeSqr)
        {
            //increase beep speed up to max depending on distance
            float normalisedDistance = Mathf.Clamp01(1 - (distance / rangeSqr));
            currentBeepDelay = Mathf.Lerp(maxBeepDelay, minBeepDelay, normalisedDistance);
        }
        else
        {
            //maintain slow beep
            currentBeepDelay = maxBeepDelay;
        }
    }

    void Beep(float distance)
    {
        GameObject currentSO = Instantiate(beepSoundObject);
        float pitch;

        float rangeSqr = range * range;
        if (distance <= rangeSqr)
        {
            //increase beep pitch up to max depending on distance
            float normalisedDistance = Mathf.Clamp01(1 - (distance / rangeSqr));
            pitch = Mathf.Lerp(minPitch, maxPitch, normalisedDistance);
        }
        else
        {
            //maintain low beep
            pitch = minPitch;
        }

        currentSO.GetComponent<SoundObject>().PlayWithSettings(pitch, volume);
        Destroy(currentSO, currentSO.GetComponent<AudioSource>().clip.length);
    }

    IEnumerator BlinkingLight()
    {
        //turn light on
        MeshRendererToUpdate.material = lightOnMaterial;

        yield return new WaitForSeconds(lightOnTime);

        //turn light off
        MeshRendererToUpdate.material = lightOffMaterial;

        StopCoroutine(BlinkingLight());

    }
}