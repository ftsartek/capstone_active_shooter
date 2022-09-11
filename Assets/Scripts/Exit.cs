using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    // end menu canvas
    EndMenu endMenu;

    void Start()
    {
        GameObject endFrame = GameObject.Find("EndFrame");
        endMenu = endFrame.GetComponent<EndMenu>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hehe");
        // deal with escaping player
        if (other.tag == "Player")
        {
            Debug.Log("heheh");
            //end game
            endMenu.ToggleEndMenu(false);

        }

        // deal with escaping NPCs
        if (other.tag == "NPC")
        {
            // make NPC disappear (attacker SHOULD NOT be tagged as NPC)
        }
    }
}
