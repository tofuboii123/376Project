using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLevel1ExitDoor : Interactable
{
    private static bool doorIsUnlocked = false;

    private void Start()
    {
        doorIsUnlocked = false;
    }

    public override void OnInteraction()
    {
        if (!doorIsUnlocked)
        {
            if (inventory.ContainsSelectedItem(15))
            {
                inventory.DiscardItem(15);
                doorIsUnlocked = true;
                MessageController.ShowMessage("I unlocked the door to the stairs. I can access the first floor now.", Face.Happy);
                interactTextString = "Enter first floor";
            }
            else
            {
                MessageController.ShowMessage("Looks like this door leads to the first floor staircase. I need to unlock it.", Face.Thinking);
            }
        }
        else
        {
            Level1Controller level1Controller = GameObject.Find("Level1Controller").GetComponent<Level1Controller>();
            StartCoroutine(level1Controller.LevelEnd());
        }
    }
}
