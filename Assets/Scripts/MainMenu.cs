using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public PostProcessVolume volume;
    private DepthOfField depthOfField;
    private Bloom bloom;

    public Image image;

    public GameObject headingText;
    public GameObject playButton;
    public GameObject demoButton;
    public GameObject helpButton;
    public GameObject creditsButton;
    public GameObject optionsButton;
    public GameObject exitButton;

    public GameObject helpScreen;
    public GameObject optionsScreen;

    public TextMeshProUGUI masterVolumeText;
    public Slider masterVolumeSlider;

    private AudioManager audioManager;

    void Awake() {
        headingText.SetActive(true);
        playButton.SetActive(true);
        demoButton.SetActive(true);
        helpButton.SetActive(true);
        creditsButton.SetActive(true);
        exitButton.SetActive(true);

        helpScreen.SetActive(false);
    }

    void Start() {
        StartCoroutine(FadeOut());

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

        headingText.SetActive(false);
        playButton.SetActive(false);
        demoButton.SetActive(false);
        helpButton.SetActive(false);
        creditsButton.SetActive(false);
        optionsButton.SetActive(false);
        exitButton.SetActive(false);

        helpScreen.SetActive(true);
    }

    public void ExitHelp() {
        GetAudioManager();
        audioManager.Play("Mouse Click");

        headingText.SetActive(true);
        playButton.SetActive(true);
        demoButton.SetActive(true);
        helpButton.SetActive(true);
        creditsButton.SetActive(true);
        optionsButton.SetActive(true);
        exitButton.SetActive(true);

        helpScreen.SetActive(false);
    }

    public void Credits()
    {

        GetAudioManager();
        audioManager.Play("Mouse Click");
        audioManager.StopFadeOut("Main Menu Music", 2.0f);

        StartCoroutine(StartCredits());
    }

    public void Options() {
        GetAudioManager();
        audioManager.Play("Mouse Click");

        headingText.SetActive(false);
        playButton.SetActive(false);
        demoButton.SetActive(false);
        helpButton.SetActive(false);
        creditsButton.SetActive(false);
        optionsButton.SetActive(false);
        exitButton.SetActive(false);

        optionsScreen.SetActive(true);

        int masterVolumeSet = Option.GetVolumePercent();
        masterVolumeSlider.value = masterVolumeSet / 100.0f;
        masterVolumeText.text = masterVolumeSet + "%";
    }

    public void ExitOptions() {
        GetAudioManager();
        audioManager.Play("Mouse Click");

        headingText.SetActive(true);
        playButton.SetActive(true);
        demoButton.SetActive(true);
        helpButton.SetActive(true);
        creditsButton.SetActive(true);
        optionsButton.SetActive(true);
        exitButton.SetActive(true);

        optionsScreen.SetActive(false);
    }

    public void NewVolume(float newVolume) {
        int volume = (int)(newVolume * 100);
        masterVolumeText.text = volume + "%";

        Option.SetVolumePercent(volume);
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

    IEnumerator StartCredits()
    {
        float timer;

        timer = 0.0f;
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;

            image.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, timer / 0.5f));

            yield return null;
        }

        SceneManager.LoadScene("Credits");
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

    IEnumerator FadeOut()
    {
        // loop over 1 second backwards
        for (float i = 2; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            image.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }

    private void GetAudioManager() {
        if (audioManager == null) {
            audioManager = FindObjectOfType<AudioManager>();
        }
    }
}