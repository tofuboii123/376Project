using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableRadio : Interactable
{
    public static bool heardNumbers = false;
    private bool hasBattery = false;

    private void Start()
    {
        heardNumbers = false;
    }

    public override void OnInteraction()
    {
        if (hasBattery)
        {
            heardNumbers = true;
            MessageController.ShowMessage(new string[] {
                "The radio keeps playing back the same sequence of numbers over and over.",
                "I wonder if I'm supposed to use them somewhere." }, new int[] {
                Face.Surprised,
                Face.Thinking});
        }
        else
        {
            if (inventory.ContainsSelectedItem(211))
            {
                inventory.DiscardItem(211);
                hasBattery = true;
                MessageController.ShowMessage("I put the battery in the radio. It should work now.", Face.Happy);
            }
            else
            {
                MessageController.ShowMessage(new string[] {
                    "This radio isn't working.",
                    "I think it's missing a battery." }, new int[] {
                    Face.Disappointed,
                    Face.Thinking
                });
            }
        }
    }
}
