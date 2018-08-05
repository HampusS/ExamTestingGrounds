using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioM : MonoBehaviour {
    public static AudioM instance;
    public static AudioM Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioM>();

                if (instance == null)
                {
                    GameObject container = new GameObject("Player");
                    instance = container.AddComponent<AudioM>();
                }
            }

            return instance;
        }
    }

    public Sound[] sounds;
    void Awake () {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        //Play("miami");
	}
	//Call sound with FindObjectOfType<AudioManager>().play("nameOfclip");
	public void Play (string name) {
        Sound s = FindSound(name);
        if (s == null)
        {
            Debug.LogWarning("Sound" + name + "not found");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = FindSound(name);
        if (s == null)
            return;
        s.source.Stop();
    }

    public Sound FindSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s;
    }
}
