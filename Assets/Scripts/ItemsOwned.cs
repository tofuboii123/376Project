using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsOwned : MonoBehaviour {
    public static List<int> items;
    public static List<int> itemsQuantity;
    public static List<Sprite> itemsSprite;

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

        for (int i = 0; i < totalInventorySize; i++) {
            items.Add(-1);
            itemsQuantity.Add(0);
            itemsSprite.Add(null);
        }

        selectedItemIndex = 0;
    }
}