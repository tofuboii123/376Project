using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableChest : Interactable
{
    public override void OnInteraction() {
        MessageController.ShowMessage("There's nothing in here.");
        canInteract = false;
    }
}
