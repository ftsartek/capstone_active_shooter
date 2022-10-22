using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
  public float maxHealth;
  [HideInInspector]
  public float currentHealth;
  Ragdoll ragdoll;
  EndMenuScript endMenu;
    // Start is called before the first frame update
    void Start()
    {
      GameObject endFrame = GameObject.Find("EndFrame");
        endMenu = endFrame.GetComponent<EndMenuScript>();
        ragdoll = GetComponent<Ragdoll>();
        currentHealth = maxHealth;

        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rigidBody in rigidBodies) {
          HitBox hitBox = rigidBody.gameObject.AddComponent<HitBox>();
          hitBox.health = this;
          if (hitBox.gameObject != gameObject) {
            hitBox.gameObject.layer = LayerMask.NameToLayer("Hitbox");
          }
        }
    }

    public void TakeDamage(float amount, Vector3 direction) {
      currentHealth -= amount;
      if (currentHealth <= 0.0f) {
        Die();
      }
    }

    private void Die() {
      ragdoll.ActivateRagDoll();
      
      endMenu.ToggleEndMenu(true);

    }

}
