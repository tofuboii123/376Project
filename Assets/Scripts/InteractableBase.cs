using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected Inventory inventory;
    protected TextMeshProUGUI interactTextObject;
    public string interactTextString;
    public abstract void OnInteraction();

    protected bool canInteract = false;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        inventory = player.GetComponent<Inventory>();
        interactTextObject = player.GetComponent<PlayerController>().interactTextObject;

        interactTextObject.text = "";
        interactTextObject.enabled = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && canInteract)
        {
            OnInteraction();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerItemCollider"))
        {
            interactTextObject.text = "X - " + interactTextString;
            interactTextObject.enabled = true;

            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerItemCollider"))
        {
            interactTextObject.text = "";
            interactTextObject.enabled = false;

            canInteract = false;
        }
    }
}

public class InteractableBase : MonoBehaviour
{

}
