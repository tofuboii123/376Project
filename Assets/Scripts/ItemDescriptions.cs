using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ItemInformation {
    public string itemName;
    public string itemDescription;

    public ItemInformation() {
        itemName = "";
        itemDescription = "";
    }

    public ItemInformation(string itemName, string itemDescription) {
        this.itemName = itemName;
        this.itemDescription = itemDescription;
    }
}

public class ItemDescriptions : MonoBehaviour {
    private static Dictionary<int, ItemInformation> items;

    void Start() {
        items = new Dictionary<int, ItemInformation>();

        string readFromFilePath = Application.streamingAssetsPath + "/Items/ItemInformation.txt";
        List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList();

        int itemID = -1;
        string itemName = "";
        string itemDescription = "";
        ItemInformation itemInformation;

        foreach (string line in fileLines) {
            if (line.StartsWith("@ITEM")) {
                itemID = Int32.Parse(line.Split(' ')[1]);
            } else if (line.StartsWith("#ItemName: ")) {
                itemName = line.Remove(0, 11);
            } else if (line.StartsWith("#Description: ")) {
                itemDescription = line.Remove(0, 14);
            } else if (line.StartsWith("@ENDITEM")) {
                itemInformation = new ItemInformation(itemName, itemDescription);
                items.Add(itemID, itemInformation);
            }
        }
    }

    public static ItemInformation GetItemDescription(int itemID) {
        if (items.ContainsKey(itemID)) {
            return items[itemID];
        }

        return new ItemInformation();
    }
}