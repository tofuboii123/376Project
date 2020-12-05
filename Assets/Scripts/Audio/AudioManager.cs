// Inspired by Brackeys video: https://www.youtube.com/watch?v=6OT43pvUyfY
// Added functionality + optimized to use a Dictionary instead of array for playing sounds

using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public Sound[] sounds;
    private Dictionary<string, Sound> soundsList;

    public bool disableAllSounds;

    public static AudioManager instance;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        soundsList = new Dictionary<string, Sound>();

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;

            soundsList.Add(s.name, s);
        }
    }

    public void Play(string name) {
        Sound s = FindSoundByName(name);
        if (s == null) {
            return;
        }

        s.source.volume = s.volume;
        s.source.Play();
    }

    public void PlayIfNotPlaying(string name) {
        Sound s = FindSoundByName(name);
        if (s == null) {
            return;
        }

        if (s.source.isPlaying) {
            return;
        }

        s.source.volume = s.volume;
        s.source.Play();
    }

    public void PlayFadeIn(string name) {
        PlayFadeIn(name, 1.0f);
    }

    public void PlayFadeIn(string name, float secondsToFadeIn) {
        Sound s = FindSoundByName(name);
        if (s == null) {
            return;
        }

        AudioSource audioSource = s.source;
        float targetVolume = s.volume;
        StartCoroutine(FadeIn(audioSource, secondsToFadeIn, targetVolume));
    }

    public void Pause(string name) {
        Sound s = FindSoundByName(name);
        if (s == null) {
            return;
        }

        s.source.Pause();
    }

    public void UnPause(string name) {
        Sound s = FindSoundByName(name);
        if (s == null) {
            return;
        }

        s.source.UnPause();
    }

    public void Stop(string name) {
        Sound s = FindSoundByName(name);
        if (s == null) {
            return;
        }

        s.source.Stop();
    }

    public void StopFadeOut(string name) {
        StopFadeOut(name, 1.0f);
    }

    public void StopFadeOut(string name, float secondsToFadeOut) {
        Sound s = FindSoundByName(name);
        if (s == null) {
            return;
        }

        AudioSource audioSource = s.source;
        float startingVolume = audioSource.volume;
        StartCoroutine(FadeOut(startingVolume, audioSource, secondsToFadeOut));
    }

    IEnumerator FadeOut(float startingVolume, AudioSource audioSource, float secondsToFadeOut) {
        float timer = 0.0f;
        while (timer < secondsToFadeOut) {
            timer += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(startingVolume, 0.0f, timer / secondsToFadeOut);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();
    }

    IEnumerator FadeIn(AudioSource audioSource, float secondsToFadeIn, float targetVolume) {
        audioSource.volume = 0.0f;
        audioSource.Play();

        float timer = 0.0f;
        while (timer < secondsToFadeIn) {
            timer += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(0.0f, targetVolume, timer / secondsToFadeIn);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    private Sound FindSoundByName(string name) {
        if (soundsList == null) {
            return null;
        }

        if (disableAllSounds) {
            return null;
        }

        if (soundsList.ContainsKey(name)) {
            return soundsList[name];
        }

        Debug.LogWarning("Sound: " + name + " not found!");
        return null;
    }
}