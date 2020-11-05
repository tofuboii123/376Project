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

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        inventory = player.GetComponent<Inventory>(); // Need this for some reason?
        interactTextObject = player.GetComponent<PlayerController>().interactTextObject; // Need this for some reason?
    }

    public override void OnInteraction() {

        if (inventory.ContainsSelectedItem(keyItemName)) {
            inventory.DiscardItem(keyItemName);
            MessageController.ShowMessage("You put the plant into the ground and discard the pot.");
            GrowTree();
        } else {
            MessageController.ShowMessage("That doesn't work.");
        }
    }

    private void GrowTree() {
        tree.SetActive(true);
    }
}
