using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableGoal : Interactable
{
    [SerializeField]
    string keyItemName;

    public override void OnInteraction() {
        //if (inventory.ContainsItem(keyItemName)) {
        if (inventory.items[Inventory.selectedItemIndex] == keyItemName) {
            inventory.DiscardItem(keyItemName);
            MessageController.ShowMessage("Used " + keyItemName + ".");
        }
    }
}
