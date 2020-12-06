using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [HideInInspector]
    public List<Image> slotsBackgroundList;

    public static List<Image> slotImages;

    [HideInInspector]
    public List<TextMeshProUGUI> slotQuantities;

    public GameObject fullScreenInventory;

    [SerializeField]
    GameObject invent = null;

    [SerializeField]
    Sprite empty;
    [SerializeField]
    Sprite selected;

    public bool IsFull { get; set; }

    private void Start() {
        slotsBackgroundList = new List<Image>();
        slotImages = new List<Image>();
        slotQuantities = new List<TextMeshProUGUI>();

        int itemIdx;

        Image[] images = invent.GetComponentsInChildren<Image>();
        foreach (Image image in images) {
            if (image.name.StartsWith("SlotImage")) {
                slotImages.Add(image);

                if (Int32.TryParse(image.name.Remove(0, 9), out int num)) {
                    itemIdx = num - 1;

                    image.enabled = ItemsOwned.items[itemIdx] != -1;
                    if (image.enabled) {
                        image.sprite = ItemsOwned.itemsSprite[itemIdx];
                    }
                } else {
                    image.enabled = false;
                }
            } else if (image.name.StartsWith("SlotBackground")) {
                slotsBackgroundList.Add(image);

                DragAndDrop dragAndDropComponent = image.GetComponent<DragAndDrop>();
                if (Int32.TryParse(image.name.Remove(0, 14), out int num2)) {
                    itemIdx = num2 - 1;

                    if (ItemsOwned.items[itemIdx] != -1) {
                        dragAndDropComponent.originalItemID = ItemsOwned.dragAndDropItemID[itemIdx];
                        dragAndDropComponent.combineName = ItemsOwned.dragAndDropCombineName[itemIdx];
                        dragAndDropComponent.combinedItem = ItemsOwned.dragAndDropCombineObject[itemIdx];
                    }
                }

                TextMeshProUGUI[] texts = image.GetComponentsInChildren<TextMeshProUGUI>();
                foreach (TextMeshProUGUI text in texts) {
                    if (text.name.StartsWith("SlotQuantity")) {
                        slotQuantities.Add(text);

                        if (Int32.TryParse(text.name.Remove(0, 12), out int num)) {
                            itemIdx = num - 1;

                            text.enabled = ItemsOwned.items[itemIdx] != -1;
                            if (text.enabled) {
                                text.text = "x" + ItemsOwned.itemsQuantity[itemIdx];
                            }
                        } else {
                            text.enabled = false;
                        }
                    }
                }
            }
        }

        if (slotsBackgroundList[ItemsOwned.selectedItemIndex] != null) {
            slotsBackgroundList[ItemsOwned.selectedItemIndex].sprite = selected;
        }

        IsFull = false;
    }

    void Update() {
        CheckInventoryMovement();
    }

    private void CheckInventoryMovement() {
        if (Input.GetButtonDown("ChoseItemLeft")) {
            slotsBackgroundList[ItemsOwned.selectedItemIndex].sprite = empty;

            ItemsOwned.selectedItemIndex--;
            if (ItemsOwned.selectedItemIndex < 0) {
                ItemsOwned.selectedItemIndex = slotsBackgroundList.Count - 1;
            }

            slotsBackgroundList[ItemsOwned.selectedItemIndex].sprite = selected;
        }

        if (Input.GetButtonDown("ChoseItemRight")) {
            slotsBackgroundList[ItemsOwned.selectedItemIndex].sprite = empty;

            ItemsOwned.selectedItemIndex++;
            if (ItemsOwned.selectedItemIndex > slotsBackgroundList.Count - 1) {
                ItemsOwned.selectedItemIndex = 0;
            }

            slotsBackgroundList[ItemsOwned.selectedItemIndex].sprite = selected;
        }
    }

    // add an item to the inventory
    public void AddItem(GameObject obj, int passedIndex = -999)
    {
        bool didPassIndex = (passedIndex != -999);

        // Get information on the object
        InteractableAddToInventory item = obj.GetComponent<InteractableAddToInventory>();
        int objID = item.itemID;
        string combineName = item.combineName;
        GameObject combinedItem = item.combinedObject;

        int idx;
        bool hasItemCheck;

        // Check if we already have the item
        if (!didPassIndex) {
            idx = ItemsOwned.items.IndexOf(objID);
            hasItemCheck = (idx >= 0);
        } else {
            idx = passedIndex;
            hasItemCheck = ItemsOwned.items[idx] != -1;
        }

        if (hasItemCheck) {
            // We already have the item in our inventory
            // Just add one to the quantity, not add a whole new item
            ItemsOwned.itemsQuantity[idx]++;
            if (idx < slotsBackgroundList.Count) {
                slotQuantities[idx].text = "x" + ItemsOwned.itemsQuantity[idx];
            }
            InventoryFull.slotQuantities[idx].text = "x" + ItemsOwned.itemsQuantity[idx];

            Destroy(obj); // Object not in game world anymore
            return;
        }

        // New item adding to inventory...check to see if we have space
        // If we passed in an index to add the itme to, then we don't care about this check
        if (!didPassIndex) {
            idx = ItemsOwned.items.IndexOf(-1);
            if (idx < 0) {
                IsFull = true;
                return;
            }
        }

        // It's a new item! Add it to the first open slot
        ItemsOwned.items[idx] = objID;

        // Update image in inventory slot
        Sprite sprite = obj.GetComponent<SpriteRenderer>().sprite;
        ItemsOwned.itemsSprite[idx] = sprite;

        if (idx < slotsBackgroundList.Count) {
            slotImages[idx].sprite = sprite;
            slotImages[idx].enabled = true;
        }
        InventoryFull.slotImages[idx].sprite = sprite;
        InventoryFull.slotImages[idx].enabled = true;

        // Provide the information needed for the item combination
        if (idx < slotsBackgroundList.Count) {
            DragAndDrop itemInInventory = slotImages[idx].transform.parent.gameObject.GetComponent<DragAndDrop>();
            itemInInventory.originalItemID = ItemsOwned.items[idx];
            itemInInventory.combineName = combineName;
            itemInInventory.combinedItem = combinedItem;
        }
        DragAndDrop itemInInventoryFull = InventoryFull.slotImages[idx].transform.parent.gameObject.GetComponent<DragAndDrop>();
        itemInInventoryFull.originalItemID = ItemsOwned.items[idx];
        itemInInventoryFull.combineName = combineName;
        itemInInventoryFull.combinedItem = combinedItem;

        ItemsOwned.dragAndDropItemID[idx] = ItemsOwned.items[idx];
        ItemsOwned.dragAndDropCombineName[idx] = combineName;
        ItemsOwned.dragAndDropCombineObject[idx] = combinedItem;

        // Update item quantity
        ItemsOwned.itemsQuantity[idx]++;
        if (idx < slotsBackgroundList.Count) {
            slotQuantities[idx].text = "x" + ItemsOwned.itemsQuantity[idx];
            slotQuantities[idx].enabled = true;
        }
        InventoryFull.slotQuantities[idx].text = "x" + ItemsOwned.itemsQuantity[idx];
        InventoryFull.slotQuantities[idx].enabled = true;

        Destroy(obj); // Object not in game world anymore
    }

    public bool ContainsSelectedItem(int itemID) {
        return ItemsOwned.items[ItemsOwned.selectedItemIndex] == itemID;
    }

    public bool ContainsItem(int itemID)
    {
        foreach (int item in ItemsOwned.items)
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
        int indexOfItem = ItemsOwned.items.IndexOf(itemID);
        if (indexOfItem >= 0) {
            ItemsOwned.itemsQuantity[indexOfItem]--;
            if (ItemsOwned.itemsQuantity[indexOfItem] <= 0) {
                ItemsOwned.itemsQuantity[indexOfItem] = 0;

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

                ItemsOwned.dragAndDropItemID[indexOfItem] = 0;
                ItemsOwned.dragAndDropCombineName[indexOfItem] = "";
                ItemsOwned.dragAndDropCombineObject[indexOfItem] = null;

                if (indexOfItem < slotsBackgroundList.Count) {
                    slotQuantities[indexOfItem].text = "x" + ItemsOwned.itemsQuantity[indexOfItem];
                    slotQuantities[indexOfItem].enabled = false;
                }
                InventoryFull.slotQuantities[indexOfItem].text = "x" + ItemsOwned.itemsQuantity[indexOfItem];
                InventoryFull.slotQuantities[indexOfItem].enabled = false;

                if (indexOfItem < slotsBackgroundList.Count) {
                    slotImages[indexOfItem].enabled = false;
                    slotImages[indexOfItem].sprite = null;
                }
                InventoryFull.slotImages[indexOfItem].enabled = false;
                InventoryFull.slotImages[indexOfItem].sprite = null;

                ItemsOwned.items[indexOfItem] = -1;
            } else {
                if (indexOfItem < slotsBackgroundList.Count) {
                    slotQuantities[indexOfItem].text = "x" + ItemsOwned.itemsQuantity[indexOfItem];
                    slotQuantities[indexOfItem].enabled = true;
                }
                InventoryFull.slotQuantities[indexOfItem].text = "x" + ItemsOwned.itemsQuantity[indexOfItem];
                InventoryFull.slotQuantities[indexOfItem].enabled = true;
            }
        }
    }

    public void SwapItems(int idx1, int idx2) {
        // Store temp variables for the switch
        int tempItemID = ItemsOwned.items[idx1];
        int tempItemQuantity = ItemsOwned.itemsQuantity[idx1];
        Sprite tempItemSprite = ItemsOwned.itemsSprite[idx1];

        int tempItemsOwnedDragAndDropItemID = ItemsOwned.dragAndDropItemID[idx1];
        string tempItemsOwnedDragAndDropCombineName = ItemsOwned.dragAndDropCombineName[idx1];
        GameObject tempItemsOwnedDragAndDropCombineObject = ItemsOwned.dragAndDropCombineObject[idx1];

        Sprite tempItemImage = InventoryFull.slotImages[idx1].sprite;

        DragAndDrop tempDragAndDrop = InventoryFull.slotImages[idx1].transform.parent.gameObject.GetComponent<DragAndDrop>();
        int tempDragAndDropItemID = tempDragAndDrop.originalItemID;
        string tempDragAndDropCombineName = tempDragAndDrop.combineName;
        GameObject tempDragAndDropCombineItem = tempDragAndDrop.combinedItem;

        DragAndDrop dragAndDropTo = InventoryFull.slotImages[idx2].transform.parent.gameObject.GetComponent<DragAndDrop>();

        ItemsOwned.items[idx1] = ItemsOwned.items[idx2];
        ItemsOwned.itemsQuantity[idx1] = ItemsOwned.itemsQuantity[idx2];
        ItemsOwned.itemsSprite[idx1] = ItemsOwned.itemsSprite[idx2];

        ItemsOwned.dragAndDropItemID[idx1] = ItemsOwned.dragAndDropItemID[idx2];
        ItemsOwned.dragAndDropCombineName[idx1] = ItemsOwned.dragAndDropCombineName[idx2];
        ItemsOwned.dragAndDropCombineObject[idx1] = ItemsOwned.dragAndDropCombineObject[idx2];

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

        ItemsOwned.items[idx2] = tempItemID;
        ItemsOwned.itemsQuantity[idx2] = tempItemQuantity;
        ItemsOwned.itemsSprite[idx2] = tempItemSprite;

        ItemsOwned.dragAndDropItemID[idx2] = tempItemsOwnedDragAndDropItemID;
        ItemsOwned.dragAndDropCombineName[idx2] = tempItemsOwnedDragAndDropCombineName;
        ItemsOwned.dragAndDropCombineObject[idx2] = tempItemsOwnedDragAndDropCombineObject;

        if (idx2 < slotsBackgroundList.Count) {
            slotImages[idx2].sprite = tempItemImage;
        }
        InventoryFull.slotImages[idx2].sprite = tempItemImage;

        if (idx1 < slotsBackgroundList.Count) {
            slotQuantities[idx1].text = "x" + ItemsOwned.itemsQuantity[idx1];
        }
        if (idx2 < slotsBackgroundList.Count) {
            slotQuantities[idx2].text = "x" + ItemsOwned.itemsQuantity[idx2];
        }
        InventoryFull.slotQuantities[idx1].text = "x" + ItemsOwned.itemsQuantity[idx1];
        InventoryFull.slotQuantities[idx2].text = "x" + ItemsOwned.itemsQuantity[idx2];

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

        if (ItemsOwned.itemsQuantity[idx1] <= 0) {
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
