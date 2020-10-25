using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    Inventory inventory;

    Sprite sprite;

    private void Start() {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            // Add the item to inventory.
            if (Input.GetButton("Interact")) {
                AddItem(this.gameObject);

                

            }
        }
    }

    void AddItem(GameObject obj) {
        inventory.items.Add(obj.name); // Add item name to inventory
        sprite = obj.GetComponent<SpriteRenderer>().sprite;

        // Change empty slot to object image
        foreach (Image img in inventory.slots) {
            if(img.sprite.name == "empty_slot") {
                img.sprite = sprite;
                break;
            }
        }

        print("Added " + obj.name);
        Destroy(obj); // Object not in game world anymore
    }
}
