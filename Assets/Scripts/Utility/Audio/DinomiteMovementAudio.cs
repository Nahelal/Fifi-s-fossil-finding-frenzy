using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.AI;

public class DinomiteMovementAudio : MonoBehaviour
{
    NavMeshAgent agent;
    AudioSource audioSource;

    [SerializeField] float minPitch;
    [SerializeField] float maxPitch;
    [SerializeField] float maxVolume;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        AdjustPitch();
        AdjustVolume();

    }

    void AdjustPitch()
    {
        float normalizedPitch = Mathf.InverseLerp(0f, agent.speed, agent.velocity.magnitude);
        audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, normalizedPitch);
    }

    void AdjustVolume()
    {
        float normalizedVolume = Mathf.InverseLerp(0f, agent.speed, agent.velocity.magnitude);
        audioSource.volume = maxVolume * normalizedVolume;
    }
}
