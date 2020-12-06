using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level1Controller : MonoBehaviour
{
    public Image img;

    public GameObject xInteractText;

    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LevelStart());
    }

    IEnumerator LevelStart()
    {
        PlayerController.CanMove = false;
        xInteractText.SetActive(false);
        // loop over 1 second backwards
        for (float i = 2; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }

        MessageController.ShowMessage(new string[] {
            "I made it inside the mansion!",
            "Unfortunately this is the second floor.",
            "I need to find a way to the basement." }, new int[] {
            Face.Happy,
            Face.Disappointed,
            Face.Thinking
        });

        while (MessageController.showMessage > 0) {
            yield return null;
        }

        PlayerController.CanMove = true;
        xInteractText.SetActive(true);
    }

    public IEnumerator LevelEnd()
    {
        MessageController.ShowMessage(new string[] {
            "I'll go to the first floor now...",
        }, new int[] {
            Face.Thinking
        });

        while (MessageController.showMessage > 0)
        {
            yield return null;
        }

        GetAudioManager();
        audioManager.Play("Open Door");

        for (float i = 0; i <= 3; i += Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }

        SceneManager.LoadScene("Level2");
    }

    private void GetAudioManager() {
        if (audioManager == null) {
            audioManager = FindObjectOfType<AudioManager>();
        }
    }
}
