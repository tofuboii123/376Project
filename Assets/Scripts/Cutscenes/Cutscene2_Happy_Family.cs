﻿using System.Collections;
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

    public Animator animator;

     void Start() {
        animator = Father.GetComponent<Animator>();
        isActive = false;

    }

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
        animator.SetFloat("Vertical", 0);
        animator.SetFloat("Horizontal", 1);
        PlayerController.CanMove = false;
        Cutscene_Present.inPresentTriggered = false;

        Object.Destroy(PresentTrigger);

        c.GetComponent<CameraMovement>().cutscene_mode = true;

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

        MessageController.ShowMessage(new string[] { "???:\n!!!", "People still live here?!", "Benjamin:\nYes dear! and guess where we are going this weekend.\nAvondale Park!"},
        new int[] {
            Face.Surprised,
            Face.Surprised,
            Face.BHappy,
        });
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }

        MessageController.ShowMessage(new string[] { "Abigail:\nFather that's wonderful! You're the best!\nI can't wait 'til Saturday.", "Benjamin:\nHahaha let's not get too excited about that yet, \nmy dear. Why don't we ask your mother to join us?", "Victoria:\nAvondale Park? isn't that a bit far from here..?", "Benjamin:\nI know, but it's been a while since we had a family\ntrip. Plus, Abby has been wanting to go there for a \nlong time.", "Benjamin:\nI also confirmed with Dr. Larnzo. It's your break week\nand a short trip will help before starting the \nnext phase of therapy.", "Abigail:\nYes mother! please, please!", "Victoria:\nIf you say so sweetie.\nSay, how was school today?", "Benjamin:\nI hate to interrupt you ladies, but it's a quarter past\n4. I shouldn't be late to the meeting. Take care!" }, new int[] {
            Face.AHappy,
            Face.BHappy,
            Face.VNormal,
            Face.BNormal,
            Face.BNormal,
            Face.AHappy,
            Face.VHappy,
            Face.BNormal
        });
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }

        //Debug.Log(Father.transform.position.y);
        animator.SetFloat("Vertical", -1);
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Speed", 1f);

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

        MessageController.ShowMessage(new string[] { "???:\nI gotta get outta here!"  },new int[] {
            Face.Surprised
          
        });
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

        PlayerController.CanMove = false;
        yield return new WaitForSeconds(1);

        MessageController.ShowMessage(new string[] { "???:\nBack in the present... They must be dead now..." },new int[] {
            Face.Disappointed
        });
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }
        animator.SetFloat("Speed", 0.0f);

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
