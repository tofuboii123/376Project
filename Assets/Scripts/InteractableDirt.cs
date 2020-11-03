using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDirt : Interactable
{
    [SerializeField]
    string keyItemName = "Plant";
    [SerializeField]
    GameObject tree;
    [SerializeField]
    GameObject parent;

    public override void OnInteraction() {
        if (inventory.ContainsSelectedItem(keyItemName)) {
            inventory.DiscardItem(keyItemName);
            MessageController.ShowMessage("You put the plant into the ground and discard the pot.");
            GrowTree();
        } else {
            MessageController.ShowMessage("That doesn't work...");
        }
    }

    private void GrowTree() {
        Instantiate(tree);
        tree.transform.parent = parent.transform;
    }
}
