using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static List<int> items;
    public static List<int> itemsQuantity;

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

        items = new List<int>();
        itemsQuantity = new List<int>();
        for (int i = 0; i < 15; i++) {
            items.Add(-1);
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
        // Get information on the object
        InteractableAddToInventory item = obj.GetComponent<InteractableAddToInventory>();
        int objID = item.itemID;
        string combineName = item.combineName;
        GameObject combinedItem = item.combinedObject;

        // Check if we already have the item
        int idx = items.IndexOf(objID);
        if (idx >= 0) {
            // We already have the item in our inventory
            // Just add one to the quantity, not add a whole new item
            itemsQuantity[idx]++;
            slotQuantities[idx].text = "x" + itemsQuantity[idx];
            InventoryFull.slotQuantities[idx].text = "x" + itemsQuantity[idx];

            Destroy(obj); // Object not in game world anymore
            return;
        }

        // New item adding to inventory...check to see if we have space
        idx = items.IndexOf(-1);
        if (idx < 0) {
            IsFull = true;
            return;
        }

        // It's a new item! Add it to the first open slot
        items[idx] = objID;

        // Update image in inventory slot
        Sprite sprite = obj.GetComponent<SpriteRenderer>().sprite;
        if (idx < slotsBackgroundList.Count) {
            slotImages[idx].sprite = sprite;
            slotImages[idx].enabled = true;
        }
        InventoryFull.slotImages[idx].sprite = sprite;
        InventoryFull.slotImages[idx].enabled = true;

        // Provide the information needed for the item combination
        if (idx < slotsBackgroundList.Count) {
            DragAndDrop itemInInventory = slotImages[idx].transform.parent.gameObject.GetComponent<DragAndDrop>();
            itemInInventory.originalItemID = items[idx];
            itemInInventory.combineName = combineName;
            itemInInventory.combinedItem = combinedItem;
        }
        DragAndDrop itemInInventoryFull = InventoryFull.slotImages[idx].transform.parent.gameObject.GetComponent<DragAndDrop>();
        itemInInventoryFull.originalItemID = items[idx];
        itemInInventoryFull.combineName = combineName;
        itemInInventoryFull.combinedItem = combinedItem;

        // Update item quantity
        itemsQuantity[idx]++;
        if (idx < slotsBackgroundList.Count) {
            slotQuantities[idx].text = "x" + itemsQuantity[idx];
            slotQuantities[idx].enabled = true;
        }
        InventoryFull.slotQuantities[idx].text = "x" + itemsQuantity[idx];
        InventoryFull.slotQuantities[idx].enabled = true;

        Destroy(obj); // Object not in game world anymore
    }

    public bool ContainsSelectedItem(int itemID) {
        return items[selectedItemIndex] == itemID;
    }

    // Remove the item from the inventory.
    public void DiscardItem(int itemID) {
        int indexOfItem = items.IndexOf(itemID);
        if (indexOfItem >= 0) {
            itemsQuantity[indexOfItem]--;
            if (itemsQuantity[indexOfItem] <= 0) {
                itemsQuantity[indexOfItem] = 0;
                slotQuantities[indexOfItem].text = "x" + itemsQuantity[indexOfItem];
                slotQuantities[indexOfItem].enabled = false;
                InventoryFull.slotQuantities[indexOfItem].text = "x" + itemsQuantity[indexOfItem];
                InventoryFull.slotQuantities[indexOfItem].enabled = false;

                slotImages[indexOfItem].enabled = false;
                slotImages[indexOfItem].sprite = null;
                items[indexOfItem] = -1;
                InventoryFull.slotImages[indexOfItem].enabled = false;
                InventoryFull.slotImages[indexOfItem].sprite = null;
            } else {
                slotQuantities[indexOfItem].text = "x" + itemsQuantity[indexOfItem];
                slotQuantities[indexOfItem].enabled = true;
                InventoryFull.slotQuantities[indexOfItem].text = "x" + itemsQuantity[indexOfItem];
                InventoryFull.slotQuantities[indexOfItem].enabled = true;
            }
        }
    }
}
