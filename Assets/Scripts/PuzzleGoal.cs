using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGoal : MonoBehaviour
{
    [SerializeField]
    string keyItem;

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            // Get the player's inventory.
            Inventory inventory = other.GetComponent<Inventory>();
            
            // Just for testing.
            if (Input.GetButton("Interact")) {
                foreach(string s in inventory.items) {
                    if (s == keyItem) {
                        inventory.items.Remove(s); // Take out the item from the inventory.
                        print("You did it!");
                        break;
                    }
                }
            }
        }
    }

    
}
