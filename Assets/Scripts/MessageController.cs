using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Face
{
    public const int Thinking = 0;
    public const int Surprised = 1;
    public const int Disappointed = 2;
    public const int Happy = 3;
    public const int None = 4;
}

public class MessageController : MonoBehaviour
{
    public GameObject messageBox;
    public GameObject messageText;
    public GameObject closeMessageText;
    public new AudioSource audio;
    public Sprite[] faceSprites = new Sprite[5];
    public Image face;
    
    private float typeDelay = 0.025f;
    private static int faceIndex = 0;
    private static bool messageBoxActive = false;
    private static bool textIsTyping = false;
    private static bool textFinishedTyping = false;
    public static int showMessage;
    private static string textToShow = "";
    private static string[] textArray;
    private static int[] faceIndexArray;
    private string currentText = "";
    private static bool skipText = false;

    // Start is called before the first frame update
    void Start()
    {
        messageBox.SetActive(false);
        closeMessageText.SetActive(false);
        audio.GetComponent<AudioSource>();
        face.GetComponent<SpriteRenderer>();
        showMessage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (showMessage > 0)
        {
            PlayerController.CanMove = false;
            messageBox.SetActive(true);
            messageBoxActive = true;
            if(faceIndex == Face.None)
            {
                face.enabled = false;
            }
            else
            {
                face.enabled = true;
            }
            face.sprite = faceSprites[faceIndex];
            if (!textIsTyping)
            {
                StartCoroutine(ShowText());
            }

            if (textFinishedTyping && Input.GetButtonDown("Interact") && showMessage == 1)
            {
                CloseMessage();

            } else if (textFinishedTyping && Input.GetButtonDown("Interact"))
            {
                closeMessageText.SetActive(false);
                GoToNextPage();
            } else if (Input.GetButtonDown("Interact") && currentText.Length > 1)
            {
                skipText = true;
            }
        }
    }

    public static void GoToNextPage()
    {
        showMessage -= 1;
        textIsTyping = false;
        textFinishedTyping = false;
        skipText = false;
        textToShow = textArray[textArray.Length - showMessage];
        if(faceIndexArray != null && faceIndexArray.Length > 0)
            faceIndex = faceIndexArray[faceIndexArray.Length - showMessage];
    }

    public static void ShowMessage(string[] text, int[] faces = null)
    {
        if (!messageBoxActive)
        {
            showMessage = text.Length;
            textArray = text;
            textToShow = textArray[0];
            if (faces == null || faces.Length == 0)
            {
                faceIndex = 0;
                faceIndexArray = faces;
            }
            else
            {
                faceIndexArray = faces;
                faceIndex = faceIndexArray[0];
            }
        }
    }

    public static void ShowMessage(string text, int face = Face.Thinking)
    {
        if (!messageBoxActive)
        {
            showMessage = 1;
            textToShow = text;
            faceIndex = face;
        }
    }

    private IEnumerator ShowText()
    {
        textIsTyping = true;
        textFinishedTyping = false;
        for(int i = 0; i < textToShow.Length; i++)
        {
            // skip text typing
            if (skipText)
            {
                messageText.GetComponent<Text>().text = textToShow;
                break;
            }

            // only play typewriter sound for every other character
            if(i % 3 == 0)
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
        messageBoxActive = false;
        skipText = false;
    }

}
