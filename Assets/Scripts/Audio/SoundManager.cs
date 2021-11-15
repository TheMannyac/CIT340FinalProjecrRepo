using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    [SerializeField] private Sound[] musicTracks;
    public string playOnLoad;

    Sound currentTrack;

    public static SoundManager instance;
    private void Awake()
    {
        //Ensure that there is only one audio manager per scene
        if (instance == null)
            instance = this;
        else
        {
            //Play this room's theme before destroying object
            instance.PlayMusic(playOnLoad);
            Destroy(gameObject);
            Debug.Log("SOUNDMANAGER DESTROYED");
            return;
        }

        //The audio manager persists between scenes
        DontDestroyOnLoad(gameObject);

        //Initialize Sounds
        foreach (Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        //Initialize Music Tracks
        foreach (Sound s in musicTracks)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        PlayMusic(playOnLoad);
    }

     void Update()
    {
        
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }
        s.source.Play();
    }

    public void PlayMusic(string trackName)
    {
        //Debug.Log("We are trying to play " + trackName);
        Sound newSong = Array.Find(musicTracks, sound => sound.name == trackName);
        if (newSong == null)
        {
            Debug.LogWarning("Track " + trackName + " not found");
            return;
        } else if(newSong.source.isPlaying)
        {
            Debug.LogWarning("Track " + trackName + " already playing");
            return;
        } else
        {
            if (currentTrack != null && currentTrack.source.isPlaying)
                currentTrack.source.Stop();

            currentTrack = newSong;
            currentTrack.source.Play();
        }
    }

}
