using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSink : Interactable
{
    private static bool tookKey = false;
    private static bool keyInSink = false;
    private static bool filledSink = false;

    private void Start()
    {
        tookKey = false;
        keyInSink = false;
        filledSink = false;
    }

    public override void OnInteraction()
    {
        if (tookKey)
        {
            MessageController.ShowMessage("There's nothing left for me to do here.", Face.Thinking);
        }
        else if (!keyInSink)
        {
            if (filledSink)
            {
                if (inventory.ContainsSelectedItem(213))
                {
                    if (player.GetComponent<PlayerController>().inPast)
                    {
                        keyInSink = true;
                        inventory.DiscardItem(213);

                        MessageController.ShowMessage("I put the key in the sink. Maybe the vinegar will help.", Face.Thinking);
                    }
                    else
                    {
                        MessageController.ShowMessage("I don't think now is the right time to do that.", Face.Disappointed);
                    }
                }
                else
                {
                    MessageController.ShowMessage("I filled the sink with vinegar. I should be able to do something with it now.");
                }
            }
            else
            {
                if (inventory.ContainsSelectedItem(214))
                {
                    if (player.GetComponent<PlayerController>().inPast)
                    {
                        filledSink = true;
                        inventory.DiscardItem(214);

                        MessageController.ShowMessage("I filled the sink with vinegar.", Face.Thinking);
                    }
                    else
                    {
                        MessageController.ShowMessage("I don't think now is the right time to do that.", Face.Disappointed);
                    }
                }
                else
                {
                    MessageController.ShowMessage("Just an empty sink. Maybe I could use it somehow.");
                }
            }
        }
        else
        {
            if (!player.GetComponent<PlayerController>().inPast)
            {
                tookKey = true;
                inventory.AddItem(GameObject.Find("Basement Key"));

                MessageController.ShowMessage(new string[] {
                "Looks like spending all that time submerged in vinegar has removed the rust from the key!",
                "I can use it safely now." }, new int[] {
                    Face.Happy,
                    Face.Thinking
                });
            }
            else
            {
                MessageController.ShowMessage("The key is submerged in vinegar. Now is not the time to take it out.", Face.Thinking);
            }
        }
    }
}
