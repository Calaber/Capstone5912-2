using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioClip[] audioClips;

    private AudioSource aud;

    void Start()
    {
        aud = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound (int clipV)
    {
        
        AudioClip clip = audioClips[clipV]; aud.loop = false;
        if (clip != null)
        {
            if (clipV < 5 && clipV > 0) {  aud.loop = true; aud.clip = clip; aud.volume = 1.0f; aud.PlayDelayed(0.2f); }
            else { aud.PlayOneShot(clip, 1.0f); }
        }
        else { /*Debug.Log("Attempted to play missing audio clip: " + clipV);*/ }
    }
}