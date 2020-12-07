using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FullScreenInventory : MonoBehaviour {
    public PostProcessVolume volume;
    private DepthOfField depthOfField;

    public static bool inMenu;
    public static bool inHelpScreen;
    public static bool isExitingGame;

    private static FullScreenInventory instance;

    public Animator animator;

    public Image blackImage;

    public GameObject background;
    public GameObject helpScreen;

    public GameObject text;
    public GameObject clock;
    public GameObject inventory;
    public GameObject xInteractText;

    private CanvasGroup clockCanvasGroup;
    private CanvasGroup inventoryCanvasGroup;
    private CanvasGroup textCanvasGroup;
    private CanvasGroup xInteractTextCanvasGroup;

    private CanvasGroup canvasGroup;

    private AudioManager audioManager;

    void Start() {
        volume.profile.TryGetSettings(out depthOfField);

        inMenu = false;
        instance = this;

        clockCanvasGroup = clock.GetComponent<CanvasGroup>();
        inventoryCanvasGroup = inventory.GetComponent<CanvasGroup>();
        textCanvasGroup = text.GetComponent<CanvasGroup>();
        xInteractTextCanvasGroup = xInteractText.GetComponent<CanvasGroup>();

        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        helpScreen.SetActive(false);

        inHelpScreen = false;
        isExitingGame = false;
    }

    public static void startFullScreenInventory() {
        if (isExitingGame) {
            return;
        }

        inMenu = true;

        instance.GetAudioManager();
        instance.audioManager.Play("Menu Open");

        instance.canvasGroup.alpha = 1;
        instance.canvasGroup.interactable = true;
        instance.canvasGroup.blocksRaycasts = true;

        instance.animator.SetTrigger("Start");
        instance.StartCoroutine(instance.onStartFullScreenInventory());
    }

    public static void exitFullScreenInventory() {
        if (isExitingGame) {
            return;
        }

        inMenu = false;

        instance.GetAudioManager();
        instance.audioManager.Play("Menu Close");

        instance.animator.SetTrigger("Exit");
        instance.StartCoroutine(instance.onExitFullScreenInventory());
    }

    IEnumerator onStartFullScreenInventory() {
        float timer;

        depthOfField.focalLength.value = 50.0f;

        timer = 0.0f;
        while (timer < 0.5f) {
            timer += Time.deltaTime;

            clockCanvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, timer / 0.2f);
            inventoryCanvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, timer / 0.2f);
            textCanvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, timer / 0.2f);
            xInteractTextCanvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, timer / 0.2f);
            depthOfField.focalLength.value = Mathf.Lerp(50.0f, 100.0f, timer / 0.5f);

            yield return null;
        }
    }

    IEnumerator onExitFullScreenInventory() {
        float timer;

        timer = 0.0f;
        while (timer < 0.5f) {
            timer += Time.deltaTime;

            clockCanvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, timer / 0.5f);
            inventoryCanvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, timer / 0.5f);
            textCanvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, timer / 0.5f);
            xInteractTextCanvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, timer / 0.5f);
            depthOfField.focalLength.value = Mathf.Lerp(100.0f, 0.0f, timer / 0.5f);

            yield return null;
        }
    }

    public void OnHelpClicked() {
        if (isExitingGame) {
            return;
        }

        background.SetActive(false);
        helpScreen.SetActive(true);

        inHelpScreen = true;
    }

    public void OnHelpBackClicked() {
        if (isExitingGame) {
            return;
        }

        background.SetActive(true);
        helpScreen.SetActive(false);

        inHelpScreen = false;
    }

    public void OnExitGameClicked() {
        if (isExitingGame) {
            return;
        }

        StartCoroutine(ExitGame());
    }

    IEnumerator ExitGame() {
        for (float i = 0; i <= 1; i += Time.deltaTime) {
            // set color with i as alpha
            blackImage.color = new Color(0, 0, 0, i);
            yield return null;
        }

        SceneManager.LoadScene("MainMenu");
    }

    private void GetAudioManager() {
        if (audioManager == null) {
            audioManager = FindObjectOfType<AudioManager>();
        }
    }
}