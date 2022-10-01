using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioControl : MonoBehaviour {
    public static int maxBullets = 2;
    public static int maxAttacks = 4;
    [HideInInspector] public bool dead = false;

    private int bulletHits = 0;
    private int attacksMade = 0;

    private EndMenuScript endMenu;

    private void OnEnable() {
        GameObject endFrame = GameObject.Find("EndFrame");
        endMenu = endFrame.GetComponent<EndMenuScript>();

        bulletHits = 0;
    }

    private void OnCollisionEnter(Collision collision) {
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

    public void Attack() {
        attacksMade += 1;

        if (attacksMade < maxAttacks) {
            Debug.Log("Attacked!");
        }
        else {
            Debug.Log("Won!");
            endMenu.ToggleEndMenu(false);
        }
    }
}
