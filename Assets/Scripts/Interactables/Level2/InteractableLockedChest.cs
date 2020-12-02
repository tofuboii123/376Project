using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLockedChest : Interactable
{
    private static bool tookKey = false;

    private void Start()
    {
        tookKey = false;
    }

    public override void OnInteraction()
    {
        if (InteractableRadio.heardNumbers)
        {
            if (tookKey)
            {
                MessageController.ShowMessage("There's nothing left in the chest.", Face.Thinking);
            }
            else
            {
                inventory.AddItem(GameObject.Find("Rusty Key"));
                tookKey = true;

                MessageController.ShowMessage(new string[]{
                    "I opened the lock using the numbers from the radio!",
                    "There's an old rusty key in here. It's probably too fragile to use in its current state.",
                    "Maybe I can fix it somehow."}, new int[]{
                    Face.Happy,
                    Face.Disappointed,
                    Face.Thinking
                });
            }
        }
        else
        {
            MessageController.ShowMessage(new string[]{
                    "The chest has a combination lock on it.",
                    "I'll need to find the combination to open it.",}, new int[]{
                    Face.Thinking,
                    Face.Thinking
                });
        }
    }
}
