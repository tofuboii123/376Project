using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static List<int> items;
    public static List<int> itemsQuantity;

    public List<Image> slotsBackgroundList;
    public static List<Image> slotImages;
    public List<TextMeshProUGUI> slotQuantities;

    public GameObject fullScreenInventory;

    [SerializeField]
    GameObject invent = null;

    [SerializeField]
    Sprite empty;
    [SerializeField]
    Sprite selected;

    public static int selectedItemIndex = 0;

    public int totalInventorySize;

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
        for (int i = 0; i < totalInventorySize; i++) {
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
            if (idx < slotsBackgroundList.Count) {
                slotQuantities[idx].text = "x" + itemsQuantity[idx];
            }
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

    public bool ContainsItem(int itemID)
    {
        foreach (int item in Inventory.items)
        {
            if (item == itemID)
            {
                return true;
            }
        }

        return false;
    }

    // Remove the item from the inventory.
    public void DiscardItem(int itemID) {
        int indexOfItem = items.IndexOf(itemID);
        if (indexOfItem >= 0) {
            itemsQuantity[indexOfItem]--;
            if (itemsQuantity[indexOfItem] <= 0) {
                itemsQuantity[indexOfItem] = 0;

                if (indexOfItem < slotsBackgroundList.Count) {
                    DragAndDrop itemInInventory = slotImages[indexOfItem].transform.parent.gameObject.GetComponent<DragAndDrop>();
                    itemInInventory.originalItemID = 0;
                    itemInInventory.combineName = "";
                    itemInInventory.combinedItem = null;
                }
                DragAndDrop itemInInventoryFull = InventoryFull.slotImages[indexOfItem].transform.parent.gameObject.GetComponent<DragAndDrop>();
                itemInInventoryFull.originalItemID = 0;
                itemInInventoryFull.combineName = "";
                itemInInventoryFull.combinedItem = null;

                if (indexOfItem < slotsBackgroundList.Count) {
                    slotQuantities[indexOfItem].text = "x" + itemsQuantity[indexOfItem];
                    slotQuantities[indexOfItem].enabled = false;
                }
                InventoryFull.slotQuantities[indexOfItem].text = "x" + itemsQuantity[indexOfItem];
                InventoryFull.slotQuantities[indexOfItem].enabled = false;

                if (indexOfItem < slotsBackgroundList.Count) {
                    slotImages[indexOfItem].enabled = false;
                    slotImages[indexOfItem].sprite = null;
                }
                InventoryFull.slotImages[indexOfItem].enabled = false;
                InventoryFull.slotImages[indexOfItem].sprite = null;

                items[indexOfItem] = -1;
            } else {
                if (indexOfItem < slotsBackgroundList.Count) {
                    slotQuantities[indexOfItem].text = "x" + itemsQuantity[indexOfItem];
                    slotQuantities[indexOfItem].enabled = true;
                }
                InventoryFull.slotQuantities[indexOfItem].text = "x" + itemsQuantity[indexOfItem];
                InventoryFull.slotQuantities[indexOfItem].enabled = true;
            }
        }
    }

    public void SwapItems(int idx1, int idx2) {
        // Store temp variables for the switch
        int tempItemID = items[idx1];
        int tempItemQuantity = itemsQuantity[idx1];
        Sprite tempItemImage = InventoryFull.slotImages[idx1].sprite;

        DragAndDrop tempDragAndDrop = InventoryFull.slotImages[idx1].transform.parent.gameObject.GetComponent<DragAndDrop>();
        int tempDragAndDropItemID = tempDragAndDrop.originalItemID;
        string tempDragAndDropCombineName = tempDragAndDrop.combineName;
        GameObject tempDragAndDropCombineItem = tempDragAndDrop.combinedItem;

        DragAndDrop dragAndDropTo = InventoryFull.slotImages[idx2].transform.parent.gameObject.GetComponent<DragAndDrop>();

        items[idx1] = items[idx2];
        itemsQuantity[idx1] = itemsQuantity[idx2];
        if (idx1 < slotsBackgroundList.Count) {
            if (idx2 < slotsBackgroundList.Count) {
                slotImages[idx1].sprite = slotImages[idx2].sprite;
            } else {
                slotImages[idx1].sprite = InventoryFull.slotImages[idx2].sprite;
            }
        }
        if (idx2 < slotsBackgroundList.Count) {
            InventoryFull.slotImages[idx1].sprite = slotImages[idx2].sprite;
        } else {
            InventoryFull.slotImages[idx1].sprite = InventoryFull.slotImages[idx2].sprite;
        }

        tempDragAndDrop.originalItemID = dragAndDropTo.originalItemID;
        tempDragAndDrop.combineName = dragAndDropTo.combineName;
        tempDragAndDrop.combinedItem = dragAndDropTo.combinedItem;

        dragAndDropTo.originalItemID = tempDragAndDropItemID;
        dragAndDropTo.combineName = tempDragAndDropCombineName;
        dragAndDropTo.combinedItem = tempDragAndDropCombineItem;

        if (idx1 < slotsBackgroundList.Count) {
            DragAndDrop dragAndDrop1 = slotImages[idx1].transform.parent.gameObject.GetComponent<DragAndDrop>();
            dragAndDrop1.originalItemID = tempDragAndDrop.originalItemID;
            dragAndDrop1.combineName = tempDragAndDrop.combineName;
            dragAndDrop1.combinedItem = tempDragAndDrop.combinedItem;
        }
        if (idx2 < slotsBackgroundList.Count) {
            DragAndDrop dragAndDrop2 = slotImages[idx2].transform.parent.gameObject.GetComponent<DragAndDrop>();
            dragAndDrop2.originalItemID = dragAndDropTo.originalItemID;
            dragAndDrop2.combineName = dragAndDropTo.combineName;
            dragAndDrop2.combinedItem = dragAndDropTo.combinedItem;
        }

        items[idx2] = tempItemID;
        itemsQuantity[idx2] = tempItemQuantity;
        if (idx2 < slotsBackgroundList.Count) {
            slotImages[idx2].sprite = tempItemImage;
        }
        InventoryFull.slotImages[idx2].sprite = tempItemImage;

        if (idx1 < slotsBackgroundList.Count) {
            slotQuantities[idx1].text = "x" + itemsQuantity[idx1];
        }
        if (idx2 < slotsBackgroundList.Count) {
            slotQuantities[idx2].text = "x" + itemsQuantity[idx2];
        }
        InventoryFull.slotQuantities[idx1].text = "x" + itemsQuantity[idx1];
        InventoryFull.slotQuantities[idx2].text = "x" + itemsQuantity[idx2];

        // The user dragged an item to an empty slot
        if (idx2 < slotsBackgroundList.Count) {
            if (!slotImages[idx2].enabled) {
                slotQuantities[idx2].enabled = true;
                slotImages[idx2].enabled = true;
            }
        }
        if (!InventoryFull.slotImages[idx2].enabled) {
            InventoryFull.slotQuantities[idx2].enabled = true;
            InventoryFull.slotImages[idx2].enabled = true;
        }

        if (itemsQuantity[idx1] <= 0) {
            if (idx1 < slotsBackgroundList.Count) {
                if (slotImages[idx1].enabled) {
                    slotImages[idx1].enabled = false;
                    slotImages[idx1].sprite = null;

                    slotQuantities[idx1].enabled = false;
                }
            }

            if (InventoryFull.slotImages[idx1].enabled) {
                InventoryFull.slotImages[idx1].enabled = false;
                InventoryFull.slotImages[idx1].sprite = null;

                InventoryFull.slotQuantities[idx1].enabled = false;
            }
        }
    }
}
