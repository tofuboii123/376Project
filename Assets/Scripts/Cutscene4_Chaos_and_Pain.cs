﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cutscene4_Chaos_and_Pain : MonoBehaviour
{

     //Actors
    public GameObject Mother;
    public GameObject Father;
    public GameObject Player;

    //UI
    public Image img;
    public GameObject Clock;
    public GameObject ClockText;
    public GameObject Inventory;


    //Camera 
    public Camera c;

    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
     animator = Mother.GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.CompareTo("Player") == 0)
        {
           
            StartCoroutine(Cutscene_Start());

        }
    }


    IEnumerator Cutscene_Start()
    {
        PlayerController.CanMove = false;

        c.GetComponent<CameraMovement>().cutscene_mode = true;
        StartCoroutine(fadeIn());

        yield return new WaitForSeconds(0.1f);

        Clock.SetActive(false);
        ClockText.SetActive(false);
        Inventory.SetActive(false);
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(fadeOut());

        yield return new WaitForSeconds(1);

        while (c.transform.position.x < 0 && c.transform.position.y < 149)
        {
            c.transform.position = Vector3.MoveTowards(c.transform.position, new Vector3(0, 149, c.transform.position.z), Time.deltaTime * 4.5f);

            yield return null;

        }


        yield return new WaitForSeconds(1.5f);
        MessageController.ShowMessage(new string[] {"Victoria:\nWhat did the doctor say?\nWhat is going on with my baby?","Benjamin:\nMy love, you should remain calm.." });
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }

        animator.SetFloat("Vertical", -1);
        animator.SetFloat("Speed", 5);


        while (Mother.transform.position.y > Father.transform.position.y)
        {
         
            Mother.transform.Translate(0, -Time.deltaTime*2.5f, 0);
            
            yield return null;

        }
        animator.SetFloat("Vertical", 0);
        animator.SetFloat("Horizontal", 1);

        while (Mother.transform.position.x < 0.5)
        {
         
            Mother.transform.Translate(Time.deltaTime*2.5f, 0, 0);
            
            yield return null;

        }
        animator.SetFloat("Speed", 0);


        MessageController.ShowMessage(new string[] {"Victoria:\nWhat did that idiot say?! Answer me!", "Benjamin:\nListen..I will tell you\nBut you have to promise me to stay calm.", "Benjamin:\nI have talked to the doctor.\nSabrina just needs a little extra care.","Victoria:\nYou are lying to me! I know there is something\nseriously wrong with her! She's been constantly crying.\nI know she's in pain..","Benjamin:\nYou are right..I don't know how to say this..\nThe doctor told me..She's been diagnosed with \nmucoviscidosis.","Benjamin:\nThey did not explain much..She was born with this\ncondition..We need to be more patient with her.","Victoria:\nBut..what did the doctors do?\nHow long is she going to suffer?","Benjamin:\nI..I'm afraid there isn't much we or the doctors can do.\nWe need to stay with her, and stay strong.","Victoria:\nSo my innocent girl going to be in pain..\nand they are going to do nothing about it.","Victoria:\nBut how can I blame them...It was my choice.\nI gave birth to her. I made her be in pain..\nI am the cause of all the pain she is going through.","Benjamin:\nThis is absolutely nobody's fault.\nListen, this is not going to be easy for the four of\nus. I need you to keep being strong. Don't listen to\nthe voices!","Victoria:\nI chose to bring her into this world..what for?\nto suffer and get hurt..like I did.\nIsn't that true?"});
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }
        while (c.transform.position.y != Player.transform.position.y && c.transform.position.x != Player.transform.position.x)
        {
         c.transform.position = Vector3.MoveTowards(c.transform.position, new Vector3(Player.transform.position.x, Player.transform.position.y, c.transform.position.z), Time.deltaTime * 4.5f);

            yield return null;

        }


        c.GetComponent<CameraMovement>().cutscene_mode = false;

        PlayerController.isTravelling = true;
        Player.GetComponent<PlayerController>().TimeShift();

        while (PlayerController.isTravelling)
        {
            yield return null;

        }
        yield return new WaitForSeconds(0.3f);


        //clearing up
        Clock.SetActive(true);
        ClockText.SetActive(true);
        Inventory.SetActive(true);
        PlayerController.CanMove = true;
        Object.Destroy(gameObject);

    }

    IEnumerator fadeOut()
    {
        // loop over 1 second backwards
        for (float i = 1; i >= 0; i -= Time.deltaTime * 10)
        {
            // set color with i as alpha
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }
        img.color = new Color(1, 1, 1, 0);


    }

    IEnumerator fadeIn()
    {

        for (float i = 0; i <= 1; i += Time.deltaTime * 10)
        {
            // set color with i as alpha
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }

    }
}