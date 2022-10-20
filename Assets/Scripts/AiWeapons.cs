using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiWeapons : MonoBehaviour
{
  RaycastWeapon currentWeapon;
  WeaponIK WeaponIk;
  Transform currentTarget;

  private void Awake() {
    WeaponIk = GetComponent<WeaponIK>();
  }

  private void update() {
    WeaponIk.SetAimTransform(currentWeapon.raycastOrigin);

    if (currentTarget && currentWeapon) {
      Vector3 target = currentTarget.position;
      currentWeapon.UpdateWeapon(Time.deltaTime, target);
      currentWeapon.UpdateBullet(Time.deltaTime);
    }
  }

  public void SetTarget(Transform target) {
    WeaponIk.SetTragetTransform(target);
    currentTarget = target;
  }

  public void SetFiring(bool enabled) {
    if(enabled) {
      currentWeapon.StartFiring();
    } else {
      currentWeapon.StopFiring();
    }
  }
}
