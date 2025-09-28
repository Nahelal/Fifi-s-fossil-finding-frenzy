using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MCs_MP3 : MonoBehaviour
{
    public static MCs_MP3 Instance { get; private set; }

    AudioSource audioSource;
    [SerializeField] AudioClip[] songs;

    int songIndex;
    bool isPaused;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        audioSource = GetComponent<AudioSource>();

        PlayRandom();
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "DiggingMinigame" || SceneManager.GetActiveScene().name == "MainMenu") 
        { 
            isPaused = true;
        }
        else
        {
            isPaused = false;
        }

        if(!audioSource.isPlaying) { PlayNext(); }

        if(isPaused) { audioSource.Stop(); }
    }

    void PlayRandom()
    {
        songIndex = Random.Range(0, songs.Length-1);

        audioSource.clip = songs[songIndex];

        audioSource.Play();
    }

    void PlayNext()
    {
        songIndex++;
        if(songIndex >= songs.Length) {songIndex = 0;}

        audioSource.clip = songs[songIndex];

        audioSource.Play();
    }
}
