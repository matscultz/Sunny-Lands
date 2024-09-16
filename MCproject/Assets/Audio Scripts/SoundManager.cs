using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private SoundLibrary sfxLibrary;
    [SerializeField]
    private AudioSource sfx2DSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound3D(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos);
        }
    }

    public void PlaySound3D(string soundName, Vector3 pos)
    {
        PlaySound3D(sfxLibrary.GetClipFromName(soundName), pos);
    }
    
    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
    }

    public void StopSound3D(string soundName)
    {
        StopSound3D(sfxLibrary.GetClipFromName(soundName));
    }
    // Metodo per fermare un suono 3D
    public void StopSound3D(AudioClip soundName)
    {
        AudioSource[] source = FindObjectsOfType<AudioSource>();
            for(int i=0; i<source.Length; i++)
        {
            if (source[i].clip.name.Equals(soundName.name))
            {
                Destroy(source[i].gameObject);
            }
        }

    }
}
