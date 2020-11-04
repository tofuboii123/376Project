using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected Inventory inventory;
    public abstract void OnInteraction();

    private bool canInteract = false;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
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
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerItemCollider"))
        {
            canInteract = false;
        }
    }
}

public class InteractableBase : MonoBehaviour
{

}
