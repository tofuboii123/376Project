﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventoryFull : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public static List<Image> slotsBackgroundList;
    public static List<Image> slotImages;
    public static List<TextMeshProUGUI> slotQuantities;

    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;

    public Image faceImage;
    public SpriteRenderer faceImageSpriteRenderer;
    public Sprite[] faceSprites = new Sprite[2];

    [SerializeField]
    Sprite empty;
    [SerializeField]
    Sprite selected;

    //private GameObject selectedGameObject;
    private int realIdx;

    // Start is called before the first frame update
    void Start() {
        itemNameText.text = "";
        itemDescriptionText.text = "";

        slotsBackgroundList = new List<Image>();
        slotImages = new List<Image>();
        slotQuantities = new List<TextMeshProUGUI>();

        Image[] images = gameObject.GetComponentsInChildren<Image>();
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

        faceImage.sprite = faceSprites[0];
    }

    public void OnPointerEnter(PointerEventData eventData) {
        GameObject gObject = eventData.pointerCurrentRaycast.gameObject;

        if (gObject.name.StartsWith("SlotBackground")) {
            //selectedGameObject = gObject;

            if (Int32.TryParse(gObject.name.Remove(0, 14), out int idx)) {
                realIdx = idx - 1;

                slotsBackgroundList[realIdx].sprite = selected;

                if (DragAndDrop.isDragging) {
                    return;
                }

                int itemID = Inventory.items[realIdx];
                ItemInformation itemInformation = ItemDescriptions.GetItemDescription(itemID);

                if (itemInformation.itemName != "") {
                    itemNameText.text = itemInformation.itemName;
                    itemDescriptionText.text = itemInformation.itemDescription;

                    faceImage.sprite = faceSprites[1];
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        slotsBackgroundList[realIdx].sprite = empty;

        if (DragAndDrop.isDragging) {
            return;
        }

        itemNameText.text = "";
        itemDescriptionText.text = "";

        faceImage.sprite = faceSprites[0];
    }
}