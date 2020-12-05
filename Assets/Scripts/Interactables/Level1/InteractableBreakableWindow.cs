using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBreakableWindow : Interactable
{
    public static bool windowIsBroken = false;

    private void Start()
    {
        windowIsBroken = false;
    }

    public override void OnInteraction()
    {
        if (inventory.ContainsSelectedItem(14))
        {
            if (player.GetComponent<PlayerController>().inPast)
            {
                windowIsBroken = true;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                if(GameObject.FindGameObjectsWithTag("BreakableWindow").Length == 2)
                {
                    GameObject.FindGameObjectsWithTag("BreakableWindow")[0].GetComponent<BoxCollider2D>().enabled = false;
                    GameObject.FindGameObjectsWithTag("BreakableWindow")[1].GetComponent<BoxCollider2D>().enabled = false;
                }
                MessageController.ShowMessage("I broke the window with the hammer!", Face.Surprised);
            }
            else
            {
                MessageController.ShowMessage("I don't think breaking the window right now would do anything.", Face.Disappointed);
            }
        }
        else
        {
            MessageController.ShowMessage("The lock on this window is jammed. I can't open it but maybe I could break it with something.", Face.Thinking);
        }
    }
}
