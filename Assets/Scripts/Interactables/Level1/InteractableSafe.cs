using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSafe : Interactable
{
    public override void OnInteraction()
    {
        if (inventory.ContainsSelectedItem(16))
        {
            inventory.DiscardItem(16);
            inventory.AddItem(GameObject.Find("Oddly Shaped Key"));
            PlayerController.hasOddKey = true;
            MessageController.ShowMessage("I opened the safe! Looks like there's a strange key inside. I'll keep it just in case.", Face.Happy);
        }
        else
        {
            MessageController.ShowMessage("This looks like a really sturdy safe. I'll need the combination in order to open it.", Face.Thinking);
        }
    }
}
