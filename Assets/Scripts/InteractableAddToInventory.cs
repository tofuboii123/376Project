using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableAddToInventory : Interactable
{
    public override void OnInteraction()
    {
        if (!inventory.ContainsItem(gameObject) && !inventory.IsFull)
        {
            inventory.AddItem(gameObject);
            MessageController.ShowMessage("Picked up a " + gameObject.name + ".");
            print("Added " + gameObject.name);
        } else if(inventory.IsFull) // Dunno if needed, might change.
            MessageController.ShowMessage("Inventory Full.");
    }
}
