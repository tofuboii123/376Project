using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class FullScreenInventory : MonoBehaviour {
    public PostProcessVolume volume;
    private DepthOfField depthOfField;

    public static bool inMenu;
    public static bool inHelpScreen;

    private static FullScreenInventory instance;

    public Animator animator;

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
    }

    public static void startFullScreenInventory() {
        inMenu = true;

        instance.canvasGroup.alpha = 1;
        instance.canvasGroup.interactable = true;
        instance.canvasGroup.blocksRaycasts = true;

        instance.animator.SetTrigger("Start");
        instance.StartCoroutine(instance.onStartFullScreenInventory());
    }

    public static void exitFullScreenInventory() {
        inMenu = false;

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
        inHelpScreen = true;

        background.SetActive(false);
        helpScreen.SetActive(true);
    }

    public void OnHelpBackClicked() {
        inHelpScreen = false;

        background.SetActive(true);
        helpScreen.SetActive(false);
    }

    public void OnExitGameClicked() {
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}