using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableLevel2Exit : Interactable
{
    public float DestX;
    public float DestY;
    public Image img;
    private static bool doorIsUnlocked = false;

    private void Start()
    {
        doorIsUnlocked = false;
    }

    public override void OnInteraction()
    {
        if (doorIsUnlocked)
        {
            StartCoroutine(Transition());
        }
        else
        {
            if (inventory.ContainsSelectedItem(215))
            {
                doorIsUnlocked = true;
                inventory.DiscardItem(215);
                interactTextString = "Enter basement";

                MessageController.ShowMessage(new string[] {
                    "I finally unlocked the door to the basement!",
                    "Time to see what secrets this mansion hides....." }, new int[] {
                    Face.Happy,
                    Face.Thinking
                });
            }
            else
            {
                MessageController.ShowMessage(new string[] {
                    "This is it. The door to the basement.",
                    "This is where I have to go. Unfortunately the door is locked.",
                    "I must find the key."}, new int[] {
                    Face.Surprised,
                    Face.Disappointed,
                    Face.Thinking
                });
            }
        }
    }

    IEnumerator Transition()
    {

        PlayerController.CanMove = false;

        for (float i = 0; i <= 1; i += Time.deltaTime * 2)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }

        player.transform.position = new Vector3(DestX, DestY, player.transform.position.z);

        for (float i = 1; i >= 0; i -= Time.deltaTime * 2)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
        img.color = new Color(0, 0, 0, 0);
        PlayerController.CanMove = true;

    }
}
