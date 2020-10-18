using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    Inventory inventory;

    private void Start() {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            // Add the item to inventory.
            if (Input.GetButton("Interact")) {
                inventory.items.Add(this.gameObject.name);
                print("added " + this.gameObject.name);
                Destroy(gameObject);
            }
        }
    }
}
