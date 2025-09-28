using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof (AudioSource))]
public class SoundObject : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip[] clips;
    [Range(0, 1)] [SerializeField] float volume = 1;
    [SerializeField] float pitch = 1;
    [SerializeField] float pitchVariance = 0;
    [SerializeField] bool playOnAwake;
    [SerializeField] AudioMixerGroup mixerGroup;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (playOnAwake)
        {
            Play();
        }
    }

    public void Play()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        int index = Random.Range(0, clips.Length);
        audioSource.clip = clips[index];
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.pitch = pitch + Random.Range(-pitchVariance, pitchVariance);
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void PlayWithSettings(float pitch = 1, float volume = 0.75f)
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        int index = Random.Range(0, clips.Length);
        audioSource.clip = clips[index];
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.pitch = pitch + Random.Range(-pitchVariance, pitchVariance);
        audioSource.volume = volume;
        audioSource.Play();
    }

    private void Update()
    {
        if (!playOnAwake) return;

        if (!audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
