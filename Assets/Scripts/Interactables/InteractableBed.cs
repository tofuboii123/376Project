using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBed : Interactable
{
    public override void OnInteraction() {
        MessageController.ShowMessage("It's not time to go to sleep yet.");
        canInteract = false;
    }
}
