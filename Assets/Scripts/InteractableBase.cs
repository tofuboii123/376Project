using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected Inventory inventory;
    protected bool canInteract = false;

    public abstract void OnInteraction();


    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            canInteract = true;
        }
    }
}

public class InteractableBase : MonoBehaviour
{
    
}
