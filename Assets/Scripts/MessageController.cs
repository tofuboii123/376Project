using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    public GameObject messageBox;
    public GameObject messageText;
    public GameObject closeMessageText;
    public AudioClip typewriter1;
    public new AudioSource audio;

    private float typeDelay = 0.06f;
    private bool textIsTyping = false;
    private bool textFinishedTyping = false;
    private static bool showMessage;
    private static string textToShow = "";
    private string currentText = "";

    // Start is called before the first frame update
    void Start()
    {
        messageBox.SetActive(false);
        audio.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (showMessage)
        {
            PlayerController.canMove = false;
            messageBox.SetActive(true);
            if (!textIsTyping)
            {
                StartCoroutine(ShowText());
            }

            if (textFinishedTyping && Input.GetButtonDown("Interact"))
            {
                CloseMessage();
            }
        }
    }

    public static void ShowMessage(string text)
    {
        showMessage = true;
        textToShow = text;
    }

    private IEnumerator ShowText()
    {
        textIsTyping = true;
        textFinishedTyping = false;
        for(int i = 0; i < textToShow.Length + 1; i++)
        {
            audio.PlayOneShot(typewriter1);
            currentText = textToShow.Substring(0, i);
            messageText.GetComponent<Text>().text = currentText;
            yield return new WaitForSeconds(typeDelay);
        }
        textFinishedTyping = true;
    }

    private void CloseMessage()
    {
        messageBox.SetActive(false);
        PlayerController.canMove = true;
        showMessage = false;
        textIsTyping = false;
    }

}
