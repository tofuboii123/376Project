using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cutscene3_Birth : MonoBehaviour
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

    //Camera 
    public Camera c;
    // Start is called before the first frame update
    void Start()
    {

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
        c.GetComponent<CameraMovement>().cutscene_mode = true;
        PlayerController.CanMove = false;
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

        MessageController.ShowMessage(new string[] { "Victoria:\nSabrina...I like it", "Benjamin:\nI feel like the happiest man in the world\nnow my dear. Our little Sabrina will bring\nus a lot of joy."});
        while (MessageController.showMessage > 0)
        {
            yield return null;
        }

        MessageController.ShowMessage(new string[] { "Victoria:\nShush..\nBen, I think there is something wrong with her..\nWhat should we do?", "Benjamin:\nDon't panic darling. You know babies cry\na lot. Remember when Abigail was this young? We\ncouldn't sleep a single night!", "Victoria:\nBut this feels different..", "Benjamin:\nShe's probably just hungry.\nLet me get the bottle."});
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
            c.transform.position = Vector3.MoveTowards(c.transform.position, new Vector3(Player.transform.position.x, Player.transform.position.y, c.transform.position.z), Time.deltaTime * 4.5f);
            yield return null;

        }
        c.GetComponent<CameraMovement>().cutscene_mode = false;

        MessageController.ShowMessage(new string[] { "???:\nI'm not staying here for another second!" });
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
