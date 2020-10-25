using UnityEngine;
using UnityEngine.UI;

public class PuzzleGoal : MonoBehaviour
{
    [SerializeField]
    string keyItem;

    Inventory inventory;

    [SerializeField]
    bool puzzleSolved = false;

    [SerializeField]
    Sprite emptySlot;

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            // Get the player's inventory.
            inventory = other.GetComponent<Inventory>();
            puzzleSolved = SelectPuzzleItem();
        }
    }

    // Check if plant is in inventory to solve puzzle
    bool SelectPuzzleItem() {
        // Just for testing.
        if (Input.GetButton("Interact")) {
            foreach (string s in inventory.items) {
                if (s == keyItem) {
                    inventory.items.Remove(s); // Take out the item from the inventory.
                    
                    // Change image to empty slot
                    foreach(Image img in inventory.slots) {
                        if (img.sprite.name == s)
                            img.sprite = emptySlot;
                    }

                    print("You did it!");
                    return true;
                }
            }
        }
        return false;
    }
}
