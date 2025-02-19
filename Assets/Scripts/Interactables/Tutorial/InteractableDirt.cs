﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDirt : Interactable
{
    [SerializeField]
    GameObject tree;

    private void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        inventory = player.GetComponent<Inventory>(); // Need this for some reason?
        interactTextObject = player.GetComponent<PlayerController>().interactTextObject; // Need this for some reason?

        if (!inventory.ContainsItem(220)) {
            tree.SetActive(false);
        }
    }

    public override void OnInteraction() {

        if (inventory.ContainsSelectedItem(itemID)) {
            inventory.DiscardItem(itemID);
            MessageController.ShowMessage("I'll put the plant in the ground.");
            GrowTree();
        } else {
            MessageController.ShowMessage("That doesn't work.");
        }
    }

    private void GrowTree() {
        tree.SetActive(true);
    }
}
