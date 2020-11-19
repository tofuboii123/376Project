using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : Interactable
{
    public string interactTextBody;

    public override void OnInteraction() {
        MessageController.ShowMessage(interactTextBody);
        canInteract = false;
    }
}
