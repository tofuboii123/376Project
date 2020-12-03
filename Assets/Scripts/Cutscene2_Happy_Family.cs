using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cutscene2_Happy_Family : MonoBehaviour
{

    //Actors
    public GameObject Mother;
    public GameObject Father;
    public GameObject Daughter;
    public GameObject Player;

    //UI
    public Image img;
    public GameObject Clock;
    public GameObject ClockText;
    public GameObject Inventory;
    public bool isActive;
    public GameObject PresentTrigger;

    //Camera 
    public Camera c;

    //Audio
    private AudioManager audioManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.CompareTo("Player") == 0)
        {
           
            if(!isActive){
            StartCoroutine(Cutscene_Start());
            isActive = true;
           }

        }
    }


    IEnumerator Cutscene_Start()
    {

        PlayerController.inCutscene = true;
        if (Cutscene_Present.inPresentTriggered){
            while(PlayerController.isTravelling == true){
                yield return null;
            }
        }

        PlayerController.CanMove = false;
        Cutscene_Present.inPresentTriggered = false;

        Object.Destroy(PresentTrigger);

        c.GetComponent<CameraMovement>().cutscene_mode = true;
        PlayerController.CanMove = false;

        GetAudioManager();
        audioManager.Play("Cutscene Start");

        StartCoroutine(fadeIn());

        yield return new WaitForSeconds(0.1f);

        Clock.SetActive(false);
        ClockText.SetActive(false);
        Inventory.SetActive(false);
        yield return new WaitForSeconds(0.01f);
        c.transform.position = new Vector3(c.transform.position.x, 142.1f, c.transform.position.z);
        StartCoroutine(fadeOut());

        yield return new WaitForSeconds(1);

        while (c.transform.position.x > -1.06)
        {
            c.transform.Translate(-Time.deltaTime * 4, 0, 0);
            yield return null;

        }
        yield return new WaitForSeconds(0.8f);

        MessageController.ShowMessage(new string[] { "???:\nw...what?? there are people living here?!\nor used to live...", "Benjamin:\nYes dear! and guess where we are going for the weekend.\nAvondale Park!"});
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }

        MessageController.ShowMessage(new string[] { "Abigail:\nDad that is wonderful! You are the best!\nI can't wait until Saturday.", "Benjamin:\nHaha let's not get too excited about that yet, my dear.\nYour mother should agree first.", "Victoria:\nAvondale Park? isn't that a bit far from here..?", "Benjamin:\nI know, but it's been a while since we had a family\ntrip. Plus, Abby has been wanting to visit there for a \nlong time.", "Benjamin:\nI also confirmed with Dr. Larnzo. It's your break week and\na short trip will help before starting the next phase \nof therapy.", "Abigail:\nYes mom! please please!", "Victoria:\nIf you say so sweetie.\nSay, how was school today?", "Benjamin:\nI hate to interrupt you ladies, but it's 15 minutes past\n4. I shouldn't be late to the meeting. Take care." });
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }

        Debug.Log(Father.transform.position.y);

        while (c.transform.position.x != Player.transform.position.x)
        {

            if (Father.transform.position.y > 140)
            {
                Father.transform.Translate(0, -Time.deltaTime, 0);
            }
            c.transform.position = Vector3.MoveTowards(c.transform.position, new Vector3(Player.transform.position.x, Player.transform.position.y, c.transform.position.z), Time.deltaTime*4.5f);
            yield return null;

        }
        c.GetComponent<CameraMovement>().cutscene_mode = false;

        MessageController.ShowMessage(new string[] { "???:\nI'm not staying here for another second!"  });
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }

        PlayerController.isTravelling = true;
        Player.GetComponent<PlayerController>().TimeShift();

        while (PlayerController.isTravelling)
        {
            yield return null;

        }
        yield return new WaitForSeconds(1);

        MessageController.ShowMessage(new string[] { "???:\nI'm safe here..They must be dead now..." });
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }

        //clearing up
        Clock.SetActive(true);
        ClockText.SetActive(true);
        Inventory.SetActive(true);
        PlayerController.CanMove = true;
        PlayerController.inCutscene = false;
        Object.Destroy(gameObject);

    }

    IEnumerator fadeOut()
    {
        // loop over 1 second backwards
        for (float i = 1; i >= 0; i -= Time.deltaTime*10)
        {
            // set color with i as alpha
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }
        img.color = new Color(1, 1, 1, 0);


    }

    IEnumerator fadeIn()
    {

        for (float i = 0; i <= 1; i += Time.deltaTime*10)
        {
            // set color with i as alpha
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }

    }

    private void GetAudioManager() {
        if (audioManager == null) {
            audioManager = FindObjectOfType<AudioManager>();
        }
    }
}
