using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<string> items;
    public List<Image> slotsBackground;
    public List<Image> slots;
    [SerializeField]
    GameObject invent = null;
    [SerializeField]
    Sprite empty;
    [SerializeField]
    Sprite selected;
    private int lastEmptyIndex = 0; // Keep track of last empty slot
    public static int selectedItemIndex = 0;
    
    public bool IsFull { get; set; }

    private void Start() {
        slotsBackground = new List<Image>();
        slots = new List<Image>();

        Image[] s = invent.GetComponentsInChildren<Image>();
        foreach (Image i in s) {
            if (i.name.StartsWith("SlotImage")) {
                slots.Add(i);
                i.enabled = false;
            } else if (i.name.StartsWith("SlotBackground")) {
                slotsBackground.Add(i);
            }
        }

        if (slotsBackground[0] != null) {
            slotsBackground[0].sprite = selected;
        }

        IsFull = false;
    }

    void Update() {
        SelectItem();
    }

    private void SelectItem() {
        if (Input.GetButtonDown("ChoseItemLeft")) {
            slotsBackground[selectedItemIndex].sprite = empty;

            selectedItemIndex--;
            if (selectedItemIndex < 0) {
                selectedItemIndex = slotsBackground.Count - 1;
            }

            slotsBackground[selectedItemIndex].sprite = selected;
        }

        if (Input.GetButtonDown("ChoseItemRight")) {
            slotsBackground[selectedItemIndex].sprite = empty;

            selectedItemIndex++;
            if (selectedItemIndex > slotsBackground.Count - 1) {
                selectedItemIndex = 0;
            }

            slotsBackground[selectedItemIndex].sprite = selected;
        }
    }

    // add an item to the inventory
    public void AddItem(GameObject obj)
    {
        // TODO Maybe we wont need this
        if (lastEmptyIndex >= slots.Count) {
            IsFull = true;
            
            return;
        }

        items.Add(obj.name); // Add item name to inventory
        Sprite sprite = obj.GetComponent<SpriteRenderer>().sprite;
        slots[lastEmptyIndex].sprite = sprite;
        slots[lastEmptyIndex].enabled = true;
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
            slots[indexOfItem].enabled = false;
            slots[indexOfItem].sprite = null;
            items.Remove(item);
        }
        else
            print("Item not in inventory");
    }
}
