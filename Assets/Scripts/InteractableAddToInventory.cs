using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableAddToInventory : Interactable
{

    private void Update() {
        if(canInteract) {
            if (Input.GetButtonDown("Interact")) {
                OnInteraction();
            }

            canInteract = false;
        }
            
    }
    public override void OnInteraction()
    {
        print("On interaction");
        if (inventory.IsFull) {
            MessageController.ShowMessage("Inventory Full.");
        } else {
            inventory.AddItem(gameObject);
            MessageController.ShowMessage("Picked up a " + gameObject.name + ".");
        }
    }
}
