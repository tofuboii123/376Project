using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableAddToInventory : Interactable
{
    public override void OnInteraction()
    {
        if (!inventory.ContainsItem(gameObject))
        {
            inventory.AddItem(gameObject);
            // TODO replace print with MessageController.ShowMessage()
            print("Added " + gameObject.name);
        }
    }
}
