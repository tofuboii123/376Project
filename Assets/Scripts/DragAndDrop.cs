using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector2 originalPosition;
    Image image;

    void Start()
    {
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
            print(item.gameObject.name);
        }

        if (!validDrag)
        {
            image.GetComponent<RectTransform>().position = originalPosition;
        }
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
