using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour
{
    public Image img;
    public GameObject car;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Cutscene_Start());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Cutscene_Start()
    {

        yield return new WaitForSeconds(2);
        MessageController.ShowMessage(new string[] { "???:\nI feel so cold..Where...Where am I?..\nI don't remember anything...", "???\nNo..I remember something..I remember home.\n1666 Avenue Ravensheart..Hamilton. \nBut is that my home? Only one way to figure out." });
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }
        StartCoroutine(fadeOut());

        yield return new WaitForSeconds(4);

        MessageController.ShowMessage(new string[] { "???:\nTaxi!" });
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2.5f);

        while (car.transform.position.x > -1.7)
        {
            car.transform.Translate(-Time.deltaTime*4, 0, 0);
            yield return null;

        }
        car.GetComponent<Animator>().speed = 0;
        yield return new WaitForSeconds(0.3f);

        MessageController.ShowMessage(new string[] { "Driver:\nWhere are you going?", "???:\nHello sir..I am going to 1666 Avenue Ravensheart. Um, in Hamilton.", "Driver:\nThat's a one hour drive from here. It'll be 100 bucks.", "???:\nOh right, let me search my pockets...\nHuh, a pocket watch? it looks awfully familiar..\nand I have a hundrend dollar bill, that's lucky. Let's go." });
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }
        car.GetComponent<Animator>().speed = 1;

        this.gameObject.GetComponent<SpriteRenderer>().color = new Color (1, 1, 1, 0);
        StartCoroutine(fadeIn());
        while (car.transform.position.x >-25)
        {
            car.transform.Translate(-Time.deltaTime * 4.5f, 0, 0);
            yield return null;

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
}
