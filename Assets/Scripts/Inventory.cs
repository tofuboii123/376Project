using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<string> items;

    public List<Image> slotsBackgroundList;
    public List<Image> slotImages;

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

        Image[] s = invent.GetComponentsInChildren<Image>();
        foreach (Image i in s) {
            if (i.name.StartsWith("SlotImage")) {
                slotImages.Add(i);
                i.enabled = false;
            } else if (i.name.StartsWith("SlotBackground")) {
                slotsBackgroundList.Add(i);
            }
        }

        if (slotsBackgroundList[0] != null) {
            slotsBackgroundList[0].sprite = selected;
        }

        items = new List<string>();
        for (int i = 0; i < slotsBackgroundList.Count; i++) {
            items.Add(null);
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
        int idx = items.IndexOf(null);
        if (idx < 0) {
            IsFull = true;
            return;
        }

        items[idx] = obj.name;

        Sprite sprite = obj.GetComponent<SpriteRenderer>().sprite;
        slotImages[idx].sprite = sprite;

        slotImages[idx].enabled = true;

        Destroy(obj); // Object not in game world anymore
    }

    public bool ContainsSelectedItem(string item) {
        return items[selectedItemIndex] == item;
    }

    // Remove the item from the inventory.
    public void DiscardItem(string item) {
        int indexOfItem = items.IndexOf(item);
        if (indexOfItem >= 0) {
            slotImages[indexOfItem].enabled = false;
            slotImages[indexOfItem].sprite = null;
            items[indexOfItem] = null;
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
