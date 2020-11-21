using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableAddToInventory : Interactable
{

    public string combineName = null;       // The name of the item it can be combined with
    public GameObject combinedObject;       // The result of the combination
    public int requiredItemID = 0;
    public string getItemString = "I'll take this.";
    public string noItemString = "I don't have what I need.";

    public override void OnInteraction()
    {
        if(requiredItemID == 0)
        {
            string objectName = gameObject.name;
            string[] pickupMessage = {
            "This " + objectName + " could be useful.",
            "I should probably take this " + objectName + ".",
            "I have a feeling I'll need this " + objectName + ".",
            "I'll take this " + objectName + " just in case.",
            "A " + objectName + ". I could use this.",
            "I might need this " + objectName + " later.",
            "Something tells me this " + objectName + " could help.",
            "I guess I can take this " + objectName + ".",
            "A " + objectName + " could come in handy."
        };

            string randomMessage = pickupMessage[Random.Range(0, pickupMessage.Length - 1)];

            if (inventory.IsFull)
            {
                MessageController.ShowMessage("Inventory Full.");
            }
            else
            {
                inventory.AddItem(gameObject);
                MessageController.ShowMessage(randomMessage);
            }
        }
        else
        {
            if (inventory.ContainsSelectedItem(requiredItemID))
            {
                if (inventory.IsFull)
                {
                    MessageController.ShowMessage("Inventory Full.");
                }
                else
                {
                    inventory.DiscardItem(requiredItemID);
                    inventory.AddItem(gameObject);
                    MessageController.ShowMessage(getItemString);
                }
            }
            else
            {
                MessageController.ShowMessage(noItemString, Face.Disappointed);
            }
        }
    }
}
