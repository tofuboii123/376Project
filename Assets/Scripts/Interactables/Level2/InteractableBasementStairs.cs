using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableBasementStairs : Interactable
{
    public Image img;
    private bool hasHeart = false;

    public override void OnInteraction()
    {
        if(inventory.ContainsItem(220))
        {
            hasHeart = true;
        }

        if (hasHeart)
        {
            StartCoroutine(Transition());
        }
        else
        {
            MessageController.ShowMessage("Now is not the time to go back. I need to finish what I started.", Face.Thinking);
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

        player.transform.position = new Vector3(15.57f, 11.65f, player.transform.position.z);

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
