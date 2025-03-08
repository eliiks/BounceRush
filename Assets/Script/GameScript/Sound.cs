using System;
using UnityEngine;

/// <summary>
/// Class that manage a sound effect source and name
/// </summary>
[Serializable]
public class Sound {
    /// <summary>
    /// The name of the sound
    /// </summary>
    [Tooltip("The name of the sound")] public string name;

    /// <summary>
    /// The audio clip source
    /// </summary>
    [Tooltip("The audio clip source")] public AudioClip clip;

    /// <summary>
    /// The volume of the sound
    /// </summary>
    [Range(0f, 1f)]
    [Tooltip("The volume of the sound")] public float volume;
    
    /// <summary>
    /// The audio source component of the sound
    /// </summary>
    [HideInInspector] public AudioSource source;
}