using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    
    [Header("Audio Stuff")]
    //public AudioClip LandingAudioClip;
    [SerializeField] GameObject sandFootstep;
    [SerializeField] GameObject gravelFootstep;
    [SerializeField] GameObject dirtFootstep;
    [SerializeField] GameObject tileFootstep;
    [SerializeField] GameObject woodFootstep;

    [SerializeField] LayerMask groundLayers;
    [SerializeField] GameObject currentSOToSpawn;

    [SerializeField] Vector3 raycastOffset;

    private void Awake()
    {
        DetectGroundType();
    }

    public void PlayFootstep(AnimationEvent animationEvent)
    {
        DetectGroundType();
        Instantiate(currentSOToSpawn, transform.position, Quaternion.identity);
    }

    void DetectGroundType()
    {
        if (Physics.Raycast(transform.position + raycastOffset, -transform.up, out RaycastHit hit, 3f, groundLayers, QueryTriggerInteraction.Ignore))
        {
            //Debug.DrawRay(transform.position + raycastOffset, -transform.up, Color.red);
            //Debug.Log(hit.collider.name + " " + hit.collider.material.name);

            switch (hit.collider.material.name)
            {
                case "Wood" + " (Instance)":
                    currentSOToSpawn = woodFootstep;
                    break;
                case "Tile" + " (Instance)":
                    currentSOToSpawn = tileFootstep;
                    break;
                case "Dirt" + " (Instance)":
                    currentSOToSpawn = dirtFootstep;
                    break;
                case "Sand" + " (Instance)":
                    currentSOToSpawn = sandFootstep;
                    break;
                default:
                    currentSOToSpawn = dirtFootstep;
                    Debug.Log("No ground type found, defaulting to Dirt Footstep SFX");
                    break;
            }
        }
    }
}
