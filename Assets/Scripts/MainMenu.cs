using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public PostProcessVolume volume;
    private DepthOfField depthOfField;
    private Bloom bloom;

    public Image image;

    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start() {
        volume.profile.TryGetSettings(out depthOfField);
        volume.profile.TryGetSettings(out bloom);

        depthOfField.focalLength.value = 80.0f;
        bloom.intensity.value = 10.0f;

        GetAudioManager();
        audioManager.Play("Main Menu Music");
    }

    public void PlayGame() {
        GetAudioManager();
        audioManager.Play("Mouse Click");
        audioManager.StopFadeOut("Main Menu Music", 2.0f);

        StartCoroutine(StartGame());
    }

    public void Help() {
        GetAudioManager();
        audioManager.Play("Mouse Click");

        Debug.Log("HELP");
    }

    public void Demo() {
        GetAudioManager();
        audioManager.Play("Mouse Click");
        audioManager.StopFadeOut("Main Menu Music", 2.0f);

        Debug.Log("DEMO");
    }

    public void QuitGame() {
        GetAudioManager();
        audioManager.Play("Mouse Click");
        audioManager.Stop("Main Menu Music");

        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void onMyPointerEnter() {
        GetAudioManager();
        audioManager.Play("Mouse Hover");
    }

    IEnumerator StartGame() {
        float timer;

        timer = 0.0f;
        while (timer < 0.5f) {
            timer += Time.deltaTime;

            image.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, timer / 0.5f));

            yield return null;
        }

        SceneManager.LoadScene("Cutscene");
    }

    private void GetAudioManager() {
        if (audioManager == null) {
            audioManager = FindObjectOfType<AudioManager>();
        }
    }
}