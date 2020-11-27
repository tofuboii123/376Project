using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLevel2Exit : Interactable
{
    private static bool doorIsUnlocked = false;

    public override void OnInteraction()
    {
        if (doorIsUnlocked)
        {
            interactTextString = "Enter basement";
            print("load basement");
        }
        else
        {
            if (inventory.ContainsSelectedItem(215))
            {
                doorIsUnlocked = true;
                inventory.DiscardItem(215);

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
}
