using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<string> items;
    public List<int> itemsQuantity;

    public List<Image> slotsBackgroundList;
    public List<Image> slotImages;
    public List<TextMeshProUGUI> slotQuantities;

    [SerializeField]
    GameObject invent = null;

    [SerializeField]
    Sprite empty;
    [SerializeField]
    Sprite selected;

    public static int selectedItemIndex = 0;
    
    public bool IsFull { get; set; }

    private void Start() {
        slotsBackgroundList = new List<Image>();
        slotImages = new List<Image>();
        slotQuantities = new List<TextMeshProUGUI>();

        Image[] images = invent.GetComponentsInChildren<Image>();
        foreach (Image image in images) {
            if (image.name.StartsWith("SlotImage")) {
                slotImages.Add(image);
                image.enabled = false;
            } else if (image.name.StartsWith("SlotBackground")) {
                slotsBackgroundList.Add(image);

                TextMeshProUGUI[] texts = image.GetComponentsInChildren<TextMeshProUGUI>();
                foreach (TextMeshProUGUI text in texts) {
                    if (text.name.StartsWith("SlotQuantity")) {
                        slotQuantities.Add(text);
                        text.enabled = false;
                    }
                }
            }
        }

        if (slotsBackgroundList[0] != null) {
            slotsBackgroundList[0].sprite = selected;
        }

        items = new List<string>();
        for (int i = 0; i < slotsBackgroundList.Count; i++) {
            items.Add(null);
            itemsQuantity.Add(0);
        }

        IsFull = false;
    }

    void Update() {
        CheckInventoryMovement();
    }

    private void CheckInventoryMovement() {
        if (Input.GetButtonDown("ChoseItemLeft")) {
            slotsBackgroundList[selectedItemIndex].sprite = empty;

            selectedItemIndex--;
            if (selectedItemIndex < 0) {
                selectedItemIndex = slotsBackgroundList.Count - 1;
            }

            slotsBackgroundList[selectedItemIndex].sprite = selected;
        }

        if (Input.GetButtonDown("ChoseItemRight")) {
            slotsBackgroundList[selectedItemIndex].sprite = empty;

            selectedItemIndex++;
            if (selectedItemIndex > slotsBackgroundList.Count - 1) {
                selectedItemIndex = 0;
            }

            slotsBackgroundList[selectedItemIndex].sprite = selected;
        }
    }

    // add an item to the inventory
    public void AddItem(GameObject obj)
    {
        int idx;

        // Check if we already have the item
        idx = items.IndexOf(obj.name);
        if (idx >= 0) {
            itemsQuantity[idx]++;
            slotQuantities[idx].text = "x" + itemsQuantity[idx];
            Destroy(obj); // Object not in game world anymore

            Debug.Log("Adding to existing item! Now has x" + itemsQuantity[idx] + " of " + obj.name);
            return;
        }

        // New item adding to inventory...check to see if we have space
        idx = items.IndexOf(null);
        if (idx < 0) {
            IsFull = true;
            return;
        }

        items[idx] = obj.name;

        Sprite sprite = obj.GetComponent<SpriteRenderer>().sprite;
        slotImages[idx].sprite = sprite;
        slotImages[idx].enabled = true;

        itemsQuantity[idx]++;
        slotQuantities[idx].text = "x" + itemsQuantity[idx];
        slotQuantities[idx].enabled = true;

        Debug.Log("Adding new item! Now has x" + itemsQuantity[idx] + " of " + obj.name);

        Destroy(obj); // Object not in game world anymore
    }

    public bool ContainsSelectedItem(string item) {
        return items[selectedItemIndex] == item;
    }

    // Remove the item from the inventory.
    public void DiscardItem(string item) {
        int indexOfItem = items.IndexOf(item);
        if (indexOfItem >= 0) {
            itemsQuantity[indexOfItem]--;
            if (itemsQuantity[indexOfItem] <= 0) {
                itemsQuantity[indexOfItem] = 0; // Probably not needed...
                slotQuantities[indexOfItem].enabled = false;

                slotImages[indexOfItem].enabled = false;
                slotImages[indexOfItem].sprite = null;
                items[indexOfItem] = null;
            } else {
                slotQuantities[indexOfItem].text = "x" + itemsQuantity[indexOfItem];
                slotQuantities[indexOfItem].enabled = true;
            }
        }
    }

    // check if inventory contains a specific item
    public bool ContainsItem(GameObject obj) {
        foreach (string s in items) {
            if (s == obj.name) {
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
}
