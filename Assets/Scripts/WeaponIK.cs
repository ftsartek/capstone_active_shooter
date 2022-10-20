using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanBone {
  public HumanBodyBones bone;
}

public class WeaponIK : MonoBehaviour
{
  public Transform targetTransform;
  public Transform aimTransform;
  public Vector3 targetOffset;
  // public Transform bone;
  public int iterations = 10;
  public float weight = 1.0f;

  public float angleLimit = 90.0f;
  public float distanceLimit = 1.5f;

  public HumanBone[] humanBones;
  Transform[] boneTransforms;

    // Start is called before the first frame update
    void Start()
    {
      Animator animator = GetComponent<Animator>();
      boneTransforms = new Transform[humanBones.Length];
      for (int i = 0; i < boneTransforms.Length; i++ ) {
        boneTransforms[i] = animator.GetBoneTransform(humanBones[i].bone);
      }

    }

    Vector3 GetTargetPosistion(){
      Vector3 targetDirection = targetTransform.position - aimTransform.position;
      Vector3 aimDirection = aimTransform.forward;
      float blendOut = 0.0f;

      float targetAngle = Vector3.Angle(targetDirection, aimDirection);
      if (targetAngle > angleLimit){
        blendOut += (targetAngle - angleLimit) / 50.0f;
      }

      float targetDistance = targetDirection.magnitude;
      if (targetDistance < distanceLimit) {
        blendOut += distanceLimit - targetDistance;
      }

      Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
      return aimTransform.position + direction;

    }

    // Update is called once per frame
    void LateUpdate()
    {
      if (aimTransform == null) {
        return;
      }

      if(targetTransform == null) {
        return;
      }
        Vector3 targetPosition = GetTargetPosistion();
        for (int i = 0 ; i < iterations; i++){
        for (int b = 0 ; b < boneTransforms.Length; b++){
          Transform bone = boneTransforms[b];
          AimAtTarget(bone, targetPosition,weight);
        }
    }
  }

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight) {
      Vector3 aimDirection = aimTransform.forward;
      Vector3 targetDirection = targetPosition - aimTransform.position;
      Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
      Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards,weight);
      bone.rotation = blendedRotation * bone.rotation;
    }

    public void SetTragetTransform(Transform target) {
      targetTransform = target;
    }

    public void SetAimTransform(Transform aim) {
      aimTransform = aim;
    }


}
