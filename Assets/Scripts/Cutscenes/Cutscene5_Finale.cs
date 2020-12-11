using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cutscene5_Finale : MonoBehaviour {
    //Actors
    public GameObject Mother;
    public GameObject Player;

public GameObject BabyInCrib;
public GameObject BabyOnTable;
    private Inventory inventory;

    //UI
    public Image img;
    public GameObject Clock;
    public GameObject ClockText;
    public GameObject Inventory;
    public Image black;

    //Camera 
    public Camera c;
    public Animator animator;

    //Audio
    private AudioManager audioManager;

    public GameObject PresentTrigger;
    private bool goodEnding;

    public static bool goodEndingTriggered;

    public bool isActive;
    void Start() {
        animator = Mother.GetComponent<Animator>();
        goodEnding = false;
        goodEndingTriggered = false;
        isActive = false;
        BabyOnTable.SetActive(false);
        inventory = Player.GetComponent<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.CompareTo("Player") == 0) {
            if (!isActive) {
                goodEnding = inventory.ContainsItem(221);

                StartCoroutine(Cutscene_Start());
                isActive = true;
            }
        }
    }

    IEnumerator Cutscene_Start() {
        animator.speed = 0.15f;
        PlayerController.inCutscene = true;
        if (Cutscene_Present.inPresentTriggered) {
            while (PlayerController.isTravelling == true) {
                yield return null;
            }
        }

        PlayerController.CanMove = false;
        Cutscene_Present.inPresentTriggered = false;

        GetAudioManager();
        audioManager.Play("Cutscene Start");

        Object.Destroy(PresentTrigger);
        c.GetComponent<CameraMovement>().cutscene_mode = true;
        StartCoroutine(fadeIn());

        yield return new WaitForSeconds(0.1f);

        Clock.SetActive(false);
        ClockText.SetActive(false);
        Inventory.SetActive(false);
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(fadeOut());

        yield return new WaitForSeconds(1);
        animator.SetFloat("Vertical", 0);
        animator.SetFloat("Horizontal", -1);
        animator.SetFloat("Speed", 2.5f);

        GetAudioManager();
        audioManager.Play("Baby Crying");

        while ((c.transform.position.y < 168.9 || c.transform.position.x > -166.1) || (Mother.transform.position.x > -168.97 || Mother.transform.position.y < 170.2)) {

            c.transform.position = Vector3.MoveTowards(c.transform.position, new Vector3(-166.2f, 168.91f, c.transform.position.z), Time.deltaTime * 4.5f);
            if (Mother.transform.position.x > -168.97f) {
                Mother.transform.Translate(-1 * Time.deltaTime, 0, 0);

            } else if (Mother.transform.position.y < 170.35f) {
                animator.SetFloat("Vertical", 1);
                animator.SetFloat("Horizontal", 0);
                Mother.transform.Translate(0, Time.deltaTime, 0);

            }

            yield return null;

        }

        animator.SetFloat("Speed", 0);
        yield return new WaitForSeconds(1);
        MessageController.ShowMessage(new string[] { "Victoria:\nMy little sweetheart..why don't you calm down?", "Victoria:\nI know it is my fault.. I am the worst mother. \nI overheard them, Sabrina. Do you know what \nthat therapist was saying?", "Victoria:\nHe told Ben that it was the side effect of my pills\nI should not have taken those pills when I was \npregnant.", "Victoria:\nBut that idiot Larzno never told me that...\nHe said these pills help stop the voices... I should \nhave known better.. I am the worst mother.. \naren't I?", "Victoria:\nShush.. calm down. Why don't you stop crying?..\nwhy don't you shut up?!", "Victoria:\nI know you are suffering. I know how to ease your pain." }, new int[]{Face.VNormal,Face.VNormal,Face.VNormal,Face.VSad,Face.VSad,Face.VHappy});
        while (MessageController.showMessage > 0) {
            yield return null;
        }


        yield return new WaitForSeconds(0.5f);
        BabyInCrib.SetActive(false);
        animator.SetFloat("Vertical", -1);
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Speed", 2.5f);
        while (Mother.transform.position.y > 168.21) {


            Mother.transform.Translate(0, -Time.deltaTime, 0);
            yield return null;

        }

        animator.SetFloat("Vertical", 0);
        animator.SetFloat("Horizontal", -1);

        while (Mother.transform.position.x > -169.84) {


            Mother.transform.Translate(-Time.deltaTime, 0, 0);
            yield return null;

        }
        animator.SetFloat("Speed", 0);
        BabyOnTable.SetActive(true);
        yield return new WaitForSeconds(1);
        MessageController.ShowMessage(new string[] { "???:\nShe's holding a knife! No!" }, new int[]{Face.Surprised});
        while (MessageController.showMessage > 0) {
            yield return null;
        }
        animator.SetBool("Stab",true);
        GetAudioManager();
        audioManager.StopFadeOut("Baby Crying", 0.5f);

        yield return new WaitForSeconds(2.5f);
        animator.SetBool("Stab",false);

        MessageController.ShowMessage(new string[] { "Victoria:\nYou no longer cry my child.. you are no longer \nsuffering.." }, new int[]{Face.VMad});
        while (MessageController.showMessage > 0) {
            yield return null;
        }
        BabyOnTable.SetActive(false);

        animator.SetFloat("Horizontal", 1);
        animator.SetFloat("Speed", 3);
        while (Mother.transform.position.x < -165) {


            Mother.transform.Translate(Time.deltaTime, 0, 0);
            yield return null;

        }
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 1);
        Mother.transform.Translate(0, 0.5f * Time.deltaTime, 0);
        animator.SetFloat("Speed", 0);
        yield return new WaitForSeconds(1.5f);

        while (c.transform.position.y != Player.transform.position.y && c.transform.position.x != Player.transform.position.x) {
            c.transform.position = Vector3.MoveTowards(c.transform.position, new Vector3(Player.transform.position.x, Player.transform.position.y, c.transform.position.z), Time.deltaTime * 4.5f);

            yield return null;

        }

        c.GetComponent<CameraMovement>().cutscene_mode = false;

        if (goodEnding) {
            // good ending

            PlayerController.isTravelling = true;
            Clock.SetActive(true);
            ClockText.SetActive(true);

            Player.GetComponent<PlayerController>().TimeShift();
            while (PlayerController.isTravelling) {
                yield return null;
            }

            PlayerController.CanMove = false;

            yield return new WaitForSeconds(0.3f);

            MessageController.ShowMessage(new string[] { "???:\nThat baby... She was never buried. She could\nnever rest in peace.. I know what I need to do\nnow." },new int[]{Face.Disappointed});
            while (MessageController.showMessage > 0) {
                yield return null;
            }

            goodEndingTriggered = true;
            Inventory.SetActive(true);
            PlayerController.CanMove = true;
            Object.Destroy(gameObject);
        } else {
            // bad ending

            MessageController.ShowMessage(new string[] { "???:\nI need to run away! She's out of her mind!" }, new int[]{Face.Surprised});
            while (MessageController.showMessage > 0) {
                yield return null;
            }


            yield return new WaitForSeconds(2.5f);
            MessageController.ShowMessage(new string[] { "???:\nWhat?! why am I still here?! What is wrong with \nmy watch? It's not working!" }, new int[]{Face.Surprised});

            animator.SetFloat("Vertical", -1);
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Speed", 2.5f);

            while (Mother.transform.position.y > 164.7) {


                Mother.transform.Translate(0, -Time.deltaTime, 0);
                yield return null;

            }
            animator.SetFloat("Vertical", 0);
            animator.SetFloat("Horizontal", 1);
            Mother.transform.Translate(0.5f * Time.deltaTime, 0, 0);
            animator.SetFloat("Speed", 0);

            while (MessageController.showMessage > 0) {
                yield return null;
            }
            MessageController.ShowMessage(new string[] { "Victoria:\nYou... You saw everything.." }, new int[]{Face.VNormal});
            while (MessageController.showMessage > 0) {
                yield return null;
            }
            black.gameObject.SetActive(true);
            StartCoroutine(fadeInBlack());
            yield return new WaitForSeconds(1.5f);
            MessageController.ShowMessage(new string[] { "That clearly wasn't the good ending...\nBye now." }, new int[] { Face.None }, false);
            while (MessageController.showMessage > 0) {
                yield return null;
            }

            //clearing up
            Clock.SetActive(true);
            ClockText.SetActive(true);
            Inventory.SetActive(true);
            PlayerController.CanMove = true;

            audioManager.StopFadeOut("In Past", 1.0f);

            yield return new WaitForSeconds(1.0f);

            SceneManager.LoadScene("MainMenu");

        }
        PlayerController.inCutscene = false;

        Object.Destroy(gameObject);
    }

    IEnumerator fadeOut() {
        // loop over 1 second backwards
        for (float i = 1; i >= 0; i -= Time.deltaTime * 10) {
            // set color with i as alpha
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }
        img.color = new Color(1, 1, 1, 0);


    }

    IEnumerator fadeIn() {

        for (float i = 0; i <= 1; i += Time.deltaTime * 10) {
            // set color with i as alpha
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }

    }
    IEnumerator fadeInBlack() {

        for (float i = 0; i <= 1; i += Time.deltaTime) {
            // set color with i as alpha
            black.color = new Color(0, 0, 0, i);
            yield return null;
        }

    }

    private void GetAudioManager() {
        if (audioManager == null) {
            audioManager = FindObjectOfType<AudioManager>();
        }
    }
}
