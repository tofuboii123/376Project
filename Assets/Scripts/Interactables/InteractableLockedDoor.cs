using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLockedDoor : Interactable
{
    public override void OnInteraction() {
        MessageController.ShowMessage("The door isn't budging... Seems to be locked.");
    }
}
