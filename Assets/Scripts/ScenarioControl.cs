using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioControl : MonoBehaviour {
    public static int maxBullets = 2;
    [HideInInspector] public bool dead = false;


    private int bulletHits = 0;
    private float timer;

    private EndMenuScript endMenu;

    void OnEnable() {
        GameObject endFrame = GameObject.Find("EndFrame");
        endMenu = endFrame.GetComponent<EndMenuScript>();

        bulletHits = 0;
    }

    void OnCollisionEnter(Collision collision) {
        if (!collision.gameObject.name.StartsWith("Bullet")) return;

        bulletHits += 1;

        if (bulletHits < maxBullets) {
            Debug.Log("Hit!");
        }
        else {
            Debug.Log("Killed!");
            dead = true;

            endMenu.ToggleEndMenu(true);
        }
    }
}
