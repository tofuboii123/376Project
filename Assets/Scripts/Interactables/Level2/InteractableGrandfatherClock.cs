using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableGrandfatherClock : Interactable
{
    private static bool isDoorUnlocked = false;
    private static bool collectedItems = false;

    private void Start()
    {
        isDoorUnlocked = false;
        collectedItems = false;
    }

    public override void OnInteraction()
    {
        if (isDoorUnlocked)
        {
            if (collectedItems)
            {
                MessageController.ShowMessage("There's nothing else in this clock.", Face.Thinking);
            }
            else
            {
                collectedItems = true;
                inventory.AddItem(GameObject.Find("Bedroom Key"));
                inventory.AddItem(GameObject.Find("Left Half of Strange Relic"));
                MessageController.ShowMessage(new string[] {
                    "I grabbed the key that was inside the clock.",
                    "There appears to be a strange item in the clock. Looks like it's incomplete. I'll take it too." }, new int[] {
                    Face.Happy,
                    Face.Thinking});
            }
        }
        else
        {
            if (inventory.ContainsSelectedItem(26))
            {
                inventory.DiscardItem(26);
                isDoorUnlocked = true;
                MessageController.ShowMessage("I was able to pry open the door with the crowbar!", Face.Happy);
            }
            else
            {
                MessageController.ShowMessage(new string[] {
                    "There's a key inside this grandfather clock but the door is stuck.",
                    "I'll need to find a way to pry it open." }, new int[] {
                    Face.Disappointed,
                    Face.Thinking
                });
            }
        }
    }
}
