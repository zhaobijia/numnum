using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{

    //Sound array
    public Sound[] sounds;


    // Start is called before the first frame update
    void Awake()
    {
        foreach(Sound sound in sounds)
        {
            //for each sound instance in the sounds array
            //manager object add one audiosource component
            //and assign this component to this sound's source variable
            //                                       
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;

            sound.source.pitch = sound.pitch;

            sound.source.loop = sound.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
            return;

        s.source.Play();

    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
            return;

        s.source.Stop();
    }

}
