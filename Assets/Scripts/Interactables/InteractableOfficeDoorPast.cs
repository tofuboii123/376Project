using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableOfficeDoorPast : Interactable
{
    public override void OnInteraction()
    {
        if (inventory.ContainsSelectedItem(13))
        {
            inventory.DiscardItem(13);
            MessageController.ShowMessage("I applied water to the door lock.");
            InteractableOfficeDoorPresent.rustIsApplied = true;
        }
        else
        {
            MessageController.ShowMessage("The door is locked. I don't think a key will help me here.");
        }
    }
}
