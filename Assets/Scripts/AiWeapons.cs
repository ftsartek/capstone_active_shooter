using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiWeapons : MonoBehaviour
{
  RaycastWeapon currentWeapon;
  WeaponIK WeaponIk;
  Transform currentTarget;
  bool weaponActive = false;

  private void Start() {
    WeaponIk = GetComponent<WeaponIK>();
    currentWeapon = GetComponentInChildren<RaycastWeapon>();
  }

  public void ActivateWeapon(){
      WeaponIk.SetAimTransform(currentWeapon.raycastOrigin);
  }

  public void DeativateWeapon(){
      WeaponIk.SetAimTransform(null);
  }

  private void Update() {
    // WeaponIk.SetAimTransform(currentWeapon.raycastOrigin);

    if (currentTarget && currentWeapon && weaponActive) {
      Vector3 target = currentTarget.position;
      currentWeapon.UpdateWeapon(Time.deltaTime, target);
      currentWeapon.UpdateBullet(Time.deltaTime);
    }
  }

  public void SetTarget(Transform target) {
    WeaponIk.SetTragetTransform(target);
    currentTarget = target;
    weaponActive = true;
  }

  public void SetFiring(bool enabled) {
    if(enabled) {
      currentWeapon.StartFiring();
    } else {
      currentWeapon.StopFiring();
    }
  }
}
