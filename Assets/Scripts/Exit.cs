using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    // end menu canvas
    EndMenuScript endMenu;

    // literally just here for the trigger unit test
    public string colliding = "";

    void Start()
    {
        GameObject endFrame = GameObject.Find("EndFrame");
        //Debug.Log("starting");
        endMenu = endFrame.GetComponent<EndMenuScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("trigger1");
        //Debug.Log(colliding);
        //Debug.Log("tigger2");
        colliding = other.tag;
        //Debug.Log(colliding);

        // deal with escaping player
        if (other.tag == "Player")
        {
            Debug.Log("Exit triggered by player");
            //end game
            endMenu.ToggleEndMenu(false);
        }

        // deal with escaping NPCs
        if (other.tag == "NPC")
        {
            // make NPC disappear
        }

        if (other.tag == "Shooter")
        {
            Debug.Log("Exit triggered by shooter");
            //end game
            endMenu.ToggleEndMenu(false);
        }
    }
}
