using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector2 originalPosition;
    Image image;

    public int originalItemID;          // Original item's ID
    int otherID;                        // Compared item's ID

    [SerializeField]
    GameObject player;                  // Used to get inventory
    Inventory inventory;
    
    public string combineName;          // Name of the object that needs to be combined (Name of the image used, would be better to name the image the same as the ID...)

    public GameObject combinedItem;     // The result of the combination

    void Start()
    {
        inventory = player.GetComponent<Inventory>();
        image = GetComponentsInChildren<Image>()[1];
        originalPosition = image.GetComponent<RectTransform>().position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        image.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool validDrag = false;
        List<RaycastResult> test = GetEventSystemRaycastResults();

        foreach (RaycastResult item in test) {

            if(item.gameObject.transform.childCount > 0) {
                Transform child = item.gameObject.transform.GetChild(0);
                print(child.gameObject.name);

                // Check if items can be combined
                validDrag = IsValidDrag(child);

                // Get the other item's ID
                otherID = child.transform.parent.GetComponent<DragAndDrop>().originalItemID;
            }
        }

        // If the combination is valid, combine the items!
        if (validDrag)
            CombineItems();
        else
            image.GetComponent<RectTransform>().position = originalPosition;
        
    }

    // Check if the combination is correct.
    bool IsValidDrag(Transform child) {
        try {
            Image img = child.GetComponent<Image>();

            if (img == null || img.sprite == null)
                return false;

            return (img.sprite.name == combineName);
        }
        catch (UnassignedReferenceException) {
            return false;
        }
    }

    // Combine the two items and add a new item
    void CombineItems() {
        print("Combination!");
        
        // Delete the 2 original items
        inventory.DiscardItem(originalItemID);
        inventory.DiscardItem(otherID);

        // Add the new item
        GameObject result = Instantiate(combinedItem);
        inventory.AddItem(result);

        image.GetComponent<RectTransform>().position = originalPosition;
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
