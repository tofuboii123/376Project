using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDirt : Interactable
{
    [SerializeField]
    string keyItemName = "Plant";
    [SerializeField]
    GameObject tree;

    private void Start() {
        tree.SetActive(false);
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>(); // Need this for some reason?
    }

    public override void OnInteraction() {

        if (inventory.ContainsSelectedItem(keyItemName)) {
            inventory.DiscardItem(keyItemName);
            MessageController.ShowMessage("I'll put the plant in the ground.");
            GrowTree();
        } else {
            MessageController.ShowMessage("That doesn't work...");
        }
    }

    private void GrowTree() {
        tree.SetActive(true);
    }
}
