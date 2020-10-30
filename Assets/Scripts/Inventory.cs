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

    private void Start() {
        slots = invent.GetComponentsInChildren<Image>();
    }

    // add an item to the inventory
    public void AddItem(GameObject obj)
    {
        items.Add(obj.name); // Add item name to inventory
        Sprite sprite = obj.GetComponent<SpriteRenderer>().sprite;

        // Change empty slot to object image
        foreach (Image img in slots)
        {
            if (img.sprite.name == "empty_slot")
            {
                img.sprite = sprite;
                break;
            }
        }

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
}
