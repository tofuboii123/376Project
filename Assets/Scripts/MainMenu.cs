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

    // Start is called before the first frame update
    void Start() {
        volume.profile.TryGetSettings(out depthOfField);
        volume.profile.TryGetSettings(out bloom);

        depthOfField.focalLength.value = 80.0f;
        bloom.intensity.value = 10.0f;
    }

    public void StartGame() {
        StartCoroutine(S());
    }

    public void Help() {
        Debug.Log("HELP");
    }

    public void Demo() {
        Debug.Log("DEMO");
    }

    public void QuitGame() {
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


    IEnumerator S() {
        float timer;

        timer = 0.0f;
        while (timer < 0.5f) {
            timer += Time.deltaTime;

            image.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, timer / 0.5f));

            yield return null;
        }

        SceneManager.LoadScene("Cutscene");
    }
}