using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableMasterBedroomPresent : Interactable
{
    public float DestX;
    public float DestY;
    public Image img;
    
    private bool doorIsBroken = false;

    public override void OnInteraction()
    {
        if (InteractableBreakableWindow.windowIsBroken)
        {
            if (!doorIsBroken)
            {
                if (inventory.ContainsSelectedItem(14))
                {
                    doorIsBroken = true;
                    inventory.DiscardItem(14);
                    MessageController.ShowMessage("I was able to break the door down since it was weakened from years of outside weather!", Face.Happy);
                }
                else
                {
                    MessageController.ShowMessage("Looks like leaving the window broken so many years caused the door to become wet and moldy. I bet I could break it now.", Face.Thinking);
                }
            }
            else
            {
                StartCoroutine(Transition());
            }
        }
        else
        {
            MessageController.ShowMessage("This door is locked. Maybe I should find another way to get in.", Face.Thinking);
        }
    }

    IEnumerator Transition()
    {

        PlayerController.CanMove = false;

        for (float i = 0; i <= 1; i += Time.deltaTime * 2)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }

        player.transform.position = new Vector3(DestX, DestY, player.transform.position.z);

        for (float i = 1; i >= 0; i -= Time.deltaTime * 2)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
        img.color = new Color(0, 0, 0, 0);
        PlayerController.CanMove = true;

    }
}
