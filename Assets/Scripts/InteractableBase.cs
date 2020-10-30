using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected Inventory inventory;

    public abstract void OnInteraction();

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetButton("Interact"))
            {
                OnInteraction();
            }
        }
    }
}

public class InteractableBase : MonoBehaviour
{
    
}
