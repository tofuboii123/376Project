using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableAddToInventory : Interactable
{
    public override void OnInteraction()
    {
        if (inventory.IsFull) {
            MessageController.ShowMessage("Inventory Full.");
            return;
        }

        inventory.AddItem(gameObject);
        MessageController.ShowMessage("Picked up a " + gameObject.name + ".");
    }
}
