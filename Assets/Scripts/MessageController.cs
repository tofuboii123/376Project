using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    public GameObject messageBox;
    public GameObject messageText;
    public GameObject closeMessageText;
    public new AudioSource audio;

    private float typeDelay = 0.04f;
    private static bool textIsTyping = false;
    private static bool textFinishedTyping = false;
    public static int showMessage;
    private static string textToShow = "";
    private static string[] textArray;
    private string currentText = "";

    // Start is called before the first frame update
    void Start()
    {
        messageBox.SetActive(false);
        closeMessageText.SetActive(false);
        audio.GetComponent<AudioSource>();
        showMessage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (showMessage > 0)
        {
            PlayerController.CanMove = false;
            messageBox.SetActive(true);
            if (!textIsTyping)
            {
                StartCoroutine(ShowText());
            }

            if (textFinishedTyping && Input.GetButtonDown("Interact") && showMessage == 1)
            {
                CloseMessage();

            } else if (textFinishedTyping && Input.GetButtonDown("Interact"))
            {
                GoToNextPage();

            }
        }
    }

    public static void GoToNextPage()
    {
        showMessage -= 1;
        textIsTyping = false;
        textFinishedTyping = false;
        textToShow = textArray[textArray.Length - showMessage];
    }

    public static void ShowMessage(string[] text)
    {
        showMessage = text.Length;
        textArray = text;
        textToShow = textArray[0];

    }

    public static void ShowMessage(string text)
    {
        showMessage = 1;
        textToShow = text;

    }

    private IEnumerator ShowText()
    {
        textIsTyping = true;
        textFinishedTyping = false;
        for(int i = 0; i < textToShow.Length; i++)
        {
            // only play typewriter sound for every other character
            if(i % 2 == 0)
                audio.Play();
            currentText = textToShow.Substring(0, i + 1);
            messageText.GetComponent<Text>().text = currentText;

            // add short pause after periods, except the last period
            if (i-1 >= 0 && textToShow[i - 1].Equals('.'))
            {
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(typeDelay);
        }
        textFinishedTyping = true;
        closeMessageText.SetActive(true);
    }

    private void CloseMessage()
    {
        messageBox.SetActive(false);
        PlayerController.CanMove = true;
        showMessage -= 1;
        textIsTyping = false;
        closeMessageText.SetActive(false);
    }

}
