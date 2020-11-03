using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableGoal : Interactable
{
    [SerializeField]
    string keyItemName;

    public override void OnInteraction() {
        if (inventory.ContainsSelectedItem(keyItemName)) {
            inventory.DiscardItem(keyItemName);
            MessageController.ShowMessage("Used " + keyItemName + ".");
        } else {
            MessageController.ShowMessage("That doesn't work.");
        }
    }
}
