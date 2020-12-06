using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsOwned : MonoBehaviour {
    public static List<int> items;
    public static List<int> itemsQuantity;
    public static List<Sprite> itemsSprite;

    public static List<int> dragAndDropItemID;
    public static List<string> dragAndDropCombineName;
    public static List<GameObject> dragAndDropCombineObject;

    public static int selectedItemIndex = 0;

    public int totalInventorySize;

    public static ItemsOwned instance;

    // Start is called before the first frame update
    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        items = new List<int>();
        itemsQuantity = new List<int>();
        itemsSprite = new List<Sprite>();

        dragAndDropItemID = new List<int>();
        dragAndDropCombineName = new List<string>();
        dragAndDropCombineObject = new List<GameObject>();

        for (int i = 0; i < totalInventorySize; i++) {
            items.Add(-1);
            itemsQuantity.Add(0);
            itemsSprite.Add(null);

            dragAndDropItemID.Add(0);
            dragAndDropCombineName.Add("");
            dragAndDropCombineObject.Add(null);
        }

        selectedItemIndex = 0;
    }
}