using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<string> items;
    public Image[] slots;
    [SerializeField]
    GameObject invent;

    private void Start() {
        slots = invent.GetComponentsInChildren<Image>();
    }
}
