using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cutscene3_Birth : MonoBehaviour
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
    public bool isActive;

    //Camera 
    public Camera c;

    //Audio
    private AudioManager audioManager;

    public GameObject PresentTrigger;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
                animator = Father.GetComponent<Animator>();
        isActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.CompareTo("Player") == 0) {

            if (!isActive) {
                StartCoroutine(Cutscene_Start());
                isActive = true;
            }

        }
    }


    IEnumerator Cutscene_Start()
    {
        animator.SetFloat("Vertical", 0);
        animator.SetFloat("Horizontal", 1);
        PlayerController.inCutscene = true;
        if (Cutscene_Present.inPresentTriggered){
            while(PlayerController.isTravelling == true){
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

        while (c.transform.position.y < 124)
        {
            c.transform.Translate(0, Time.deltaTime * 3, 0);
            yield return null;

        }
        yield return new WaitForSeconds(1);
        MessageController.ShowMessage(new string[] { "???:\nA baby... Could that be... me?","Victoria:\n\"Sabrina...\" I like it.", "???:\nOkay that's definitely not me.\nBut I feel so... close to her... and her mother.","Benjamin:\nI feel like the happiest man in the world\n right now, my dear. Our little Sabrina will bring\nus a lot of joy."},new int[] {
            Face.Surprised,
            Face.VNormal,
            Face.Thinking,
            Face.BHappy
        });
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }

        GetAudioManager();
        audioManager.Play("Baby Crying");

        yield return new WaitForSeconds(2.5f);


        MessageController.ShowMessage(new string[] { "Victoria:\nShush... She's crying again...\nShe was crying all day yesterday.\nBen, I think there is something wrong with her...", "Benjamin:\nDon't panic, darling. You know babies cry\na lot. Remember when Abigail was this age? We\ncouldn't sleep a single night!", "Victoria:\nBut this feels different..", "Benjamin:\nShe's probably just hungry.\nLet me get the bottle."},new int[] {
            Face.VNormal,
            Face.BHappy,
            Face.VNormal,
            Face.BNormal
        });
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }

   animator.SetFloat("Vertical", 0);
        animator.SetFloat("Horizontal", -1);
        animator.SetFloat("Speed", 1f);
        while (c.transform.position.y != Player.transform.position.y)
        {

         
            Father.transform.Translate(-Time.deltaTime, 0, 0);
            
            c.transform.position = Vector3.MoveTowards(c.transform.position, new Vector3(Player.transform.position.x, Player.transform.position.y, c.transform.position.z), Time.deltaTime * 4.5f);
            yield return null;

        }

        audioManager.StopFadeOut("Baby Crying", 0.5f);

        c.GetComponent<CameraMovement>().cutscene_mode = false;

        PlayerController.isTravelling = true;
        Player.GetComponent<PlayerController>().TimeShift();

        while (PlayerController.isTravelling)
        {
            yield return null;

        }
        yield return new WaitForSeconds(1);
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
    private void GetAudioManager() {
        if (audioManager == null) {
            audioManager = FindObjectOfType<AudioManager>();
        }
    }
}
