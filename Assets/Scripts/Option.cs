using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour {
    private static Option instance;

    private static int volumePercent;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Default options
        SetVolumePercent(100);
    }

    public static void SetVolumePercent(int value) {
        volumePercent = value;

        AudioListener.volume = value / 100.0f;
    }

    public static int GetVolumePercent() {
        return volumePercent;
    }
}