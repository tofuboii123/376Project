﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour
{
    public Image img;
    public GameObject car;

    public GameObject text;
    public GameObject clock;
    public GameObject inventory;
    public GameObject interactText;

    private AudioManager audioManager;

    // Start is called before the first frame update
    void Awake() {
        img.color = new Color(0, 0, 0, 1);

        text.SetActive(false);
        clock.SetActive(false);
        inventory.SetActive(false);
        interactText.SetActive(false);

        car.SetActive(false);
    }

    void Start() {
        StartCoroutine(Cutscene_Start());
    }

    IEnumerator Cutscene_Start()
    {
        yield return new WaitForSeconds(2);
        MessageController.ShowMessage(new string[] {
            "???:\n1666 Avenue Ravensheart, Hamilton...",
            "???:\nIs that really where you'll be?" }, new int[] {
                Face.None, Face.None
            });
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }
        StartCoroutine(fadeOut());

        yield return new WaitForSeconds(4);

        MessageController.ShowMessage(new string[] { "???:\nTaxi!" }, new int[] { Face.Thinking });
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }

        car.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        GetAudioManager();
        audioManager.PlayFadeIn("Car Driving", 1.5f);

        yield return new WaitForSeconds(1.0f);

        bool isFadingOutSound = false;
        while (car.transform.position.x > -1.7)
        {
            car.transform.Translate(-Time.deltaTime*4, 0, 0);

            if (car.transform.position.x <= 2.5f) {
                if (!isFadingOutSound) {
                    isFadingOutSound = true;
                    audioManager.StopFadeOut("Car Driving", 3.1f);
                }
            }

            yield return null;
        }

        isFadingOutSound = false;
        car.GetComponent<Animator>().speed = 0;
        yield return new WaitForSeconds(0.3f);

        MessageController.ShowMessage(new string[] {
            "Driver:\nWhere ya going?",
            "???:\n1666 Avenue Ravensheart. Um, in Hamilton.",
            "Driver:\nYou sure? That's a pretty long way from here. It'll cost ya.",
            "???:\nThat's not a problem. I can pay.",
            "Driver:\nIf you say so.\nSay, that's a pretty nice pocket watch you got there.",
            "Driver:\nSeems like it'd fit right in a museum.\nNot many young'uns walking around with them.",
            "???:\nYeah...\nI guess not." }, new int[] {
            Face.None,
            Face.Thinking,
            Face.None,
            Face.Happy,
            Face.None,
            Face.None,
            Face.Disappointed
            });
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }

        car.GetComponent<Animator>().speed = 1;

        GetAudioManager();
        audioManager.PlayFadeIn("Car Driving", 1.5f);

        this.gameObject.GetComponent<SpriteRenderer>().color = new Color (1, 1, 1, 0);
        StartCoroutine(fadeIn());
        while (car.transform.position.x >-25)
        {
            car.transform.Translate(-Time.deltaTime * 4.5f, 0, 0);
            yield return null;

            if (car.transform.position.x <= -20) {
                if (!isFadingOutSound) {
                    isFadingOutSound = true;
                    audioManager.StopFadeOut("Car Driving", 3.1f);
                }
            }

        }

        SceneManager.LoadScene("TutorialScene");
    }

    IEnumerator fadeOut()
    {
        // loop over 1 second backwards
        for (float i = 2; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }

    IEnumerator fadeIn()
    {
        for (float i = 0; i <= 3; i += Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }

    private void GetAudioManager() {
        if (audioManager == null) {
            audioManager = FindObjectOfType<AudioManager>();
        }
    }
}
