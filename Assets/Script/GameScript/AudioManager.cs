using System;
using UnityEngine;

/// <summary>
/// Manage sounds effects from objects of the game
/// </summary>
public class AudioManager : MonoBehaviour{

    /// <summary>
    /// List of possibles sounds
    /// </summary>
    [Tooltip("List of possibles sounds")] public Sound[] sounds;

    void Awake()
    {
        // Initialize each sounds components and properties
        foreach(Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
        }   
    }

    /// <summary>
    /// Play the sound associated to a given name
    /// </summary>
    /// <param name="name">The name of the sound to play</param>
    public void Play(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}