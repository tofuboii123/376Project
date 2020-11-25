using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class DragAndDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Image image;
    TextMeshProUGUI quantityText;

    Vector2 imageOriginalPosition;
    Vector2 quantityTextOriginalPosition;

    Vector2 quantityAndImageDiff;

    public int originalItemID;          // Original item's ID
    int otherID;                        // Compared item's ID

    [SerializeField]
    GameObject player;                  // Used to get inventory
    Inventory inventory;
    
    public string combineName;          // Name of the object that needs to be combined (Name of the image used, would be better to name the image the same as the ID...)

    public GameObject combinedItem;     // The result of the combination

    public static bool isDragging;

    void Start()
    {
        inventory = player.GetComponent<Inventory>();

        image = GetComponentsInChildren<Image>()[1];
        quantityText = GetComponentsInChildren<TextMeshProUGUI>()[0];

        imageOriginalPosition = image.GetComponent<RectTransform>().position;
        quantityTextOriginalPosition = quantityText.GetComponent<RectTransform>().position;

        quantityAndImageDiff = new Vector2(imageOriginalPosition.x - quantityTextOriginalPosition.x, imageOriginalPosition.y - quantityTextOriginalPosition.y);

        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDragging = true;

        image.GetComponent<RectTransform>().position = Input.mousePosition;
        quantityText.GetComponent<RectTransform>().position = new Vector2(Input.mousePosition.x - quantityAndImageDiff.x, Input.mousePosition.y - quantityAndImageDiff.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        if (image == null || image.sprite == null) {
            image.GetComponent<RectTransform>().position = imageOriginalPosition;
            quantityText.GetComponent<RectTransform>().position = quantityTextOriginalPosition;
            return;
        }

        bool validCombinationDrag = false;
        bool validSwapDrag = false;
        int realOriginalItemIdx = -1;
        int realNewItemIdx = -1;

        List<RaycastResult> test = GetEventSystemRaycastResults();

        foreach (RaycastResult item in test) {
            if (item.gameObject.transform.childCount > 0) {
                Transform child = item.gameObject.transform.GetChild(0);
                if (child.transform.parent.name.StartsWith("SlotBackground")) {
                    // Check if items can be combined
                    Image img = child.GetComponent<Image>();
                    validCombinationDrag = IsValidCombinationDrag(img);
                    validSwapDrag = IsValidSwapDrag(img);

                    if (Int32.TryParse(image.name.Remove(0, 9), out int originalItemIdx)) {
                        realOriginalItemIdx = originalItemIdx - 1;
                    }

                    if (Int32.TryParse(child.transform.parent.name.Remove(0, 14), out int newItemIdx)) {
                        realNewItemIdx = newItemIdx - 1;
                    }

                    // Get the other item's ID
                    otherID = child.transform.parent.GetComponent<DragAndDrop>().originalItemID;
                }
            }
        }

        // Check if the drag is valid
        if (realOriginalItemIdx != realNewItemIdx) {
            if (validCombinationDrag) {
                CombineItems();
            } else if (validSwapDrag) {
                SwapItems(realOriginalItemIdx, realNewItemIdx);
            } else {
                image.GetComponent<RectTransform>().position = imageOriginalPosition;
                quantityText.GetComponent<RectTransform>().position = quantityTextOriginalPosition;
            }
        } else {
            image.GetComponent<RectTransform>().position = imageOriginalPosition;
            quantityText.GetComponent<RectTransform>().position = quantityTextOriginalPosition;
        }
    }

    // Check if the combination is correct.
    bool IsValidCombinationDrag(Image img) {
        try {
            if (img == null || img.sprite == null)
                return false;

            return (img.sprite.name == combineName);
        }
        catch (UnassignedReferenceException) {
            return false;
        }
    }

    // Check if the swap is correct.
    bool IsValidSwapDrag(Image img) {
        try {
            //if (img == null || img.sprite == null)
                //return false;

            return true;
        } catch (UnassignedReferenceException) {
            return false;
        }
    }

    // Combine the two items and add a new item
    void CombineItems() {
        //print("Combination!");

        GameObject itemToSpawn = combinedItem;
        
        // Delete the 2 original items
        inventory.DiscardItem(originalItemID);
        inventory.DiscardItem(otherID);

        // Add the new item
        GameObject result = Instantiate(itemToSpawn);
        inventory.AddItem(result);

        image.GetComponent<RectTransform>().position = imageOriginalPosition;
        quantityText.GetComponent<RectTransform>().position = quantityTextOriginalPosition;
    }

    private void SwapItems(int idx1, int idx2) {
        //print("Swap!");

        //Debug.Log("Swapping items idx " + idx1 + " with " + idx2);
        inventory.SwapItems(idx1, idx2);

        image.GetComponent<RectTransform>().position = imageOriginalPosition;
        quantityText.GetComponent<RectTransform>().position = quantityTextOriginalPosition;
    }

    // get all UI elements that the mouse is hovering over
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
