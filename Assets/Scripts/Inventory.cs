using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<string> items;
    public Image[] slots;
    [SerializeField]
    GameObject invent = null;
    [SerializeField]
    Sprite empty;
    private int lastEmptyIndex = 0; // Keep track of last empty slot
    
    public bool IsFull { get; set; }

    private void Start() {
        slots = invent.GetComponentsInChildren<Image>();
        IsFull = false;
    }

    // add an item to the inventory
    public void AddItem(GameObject obj)
    {
        // TODO Maybe we wont need this
        if (lastEmptyIndex >= slots.Length) {
            IsFull = true;
            
            return;
        }

        items.Add(obj.name); // Add item name to inventory
        Sprite sprite = obj.GetComponent<SpriteRenderer>().sprite;
        slots[lastEmptyIndex].sprite = sprite;
        lastEmptyIndex++;

        Destroy(obj); // Object not in game world anymore
    }

    // check if inventory contains a specific item
    public bool ContainsItem(GameObject obj)
    {
        foreach (string s in items)
        {
            if (s == obj.name)
            {
                return true;
            }
        }

        return false;
    }

    // check if inventory contains a specific item
    public bool ContainsItem(string item) {
        foreach (string s in items) {
            if (s == item) {
                return true;
            }
        }
        return false;
    }

    // Remove the item from the inventory.
    public void DiscardItem(string item) {
        int indexOfItem = items.IndexOf(item);
        if (indexOfItem > 0) {
            slots[indexOfItem].sprite = empty;
            items.Remove(item);
        }
        else
            print("Item not in inventory");
    }
}
