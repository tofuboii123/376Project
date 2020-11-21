using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableOpenDoor : Interactable
{
    public float DestX;
    public float DestY;
    public Image img;
    public string lockedDoorText = "This door is locked.";
    public string unlockDoorText = "I unlocked the door.";
    public bool isLocked = true;
    public int requiredItemID = 0;
    public GameObject alsoUnlock = null;

    public override void OnInteraction()
    {
        // if no item is required
        if(requiredItemID == 0)
        {
            if (isLocked)
            {
                MessageController.ShowMessage(lockedDoorText, Face.Disappointed);
            }
            else
            {
                StartCoroutine(Transition());
            }
        }
        else
        {
            if (inventory.ContainsSelectedItem(requiredItemID))
            {
                if(alsoUnlock != null)
                {
                    alsoUnlock.GetComponent<InteractableOpenDoor>().isLocked = false;
                    alsoUnlock.GetComponent<InteractableOpenDoor>().requiredItemID = 0;
                }

                isLocked = false;
                requiredItemID = 0;
                MessageController.ShowMessage(unlockDoorText, Face.Happy);
            }
            else
            {
                MessageController.ShowMessage(lockedDoorText, Face.Disappointed);
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