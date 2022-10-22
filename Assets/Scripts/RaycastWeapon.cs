using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    class Bullet {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }

    public bool isFiring = false;
    public float fireRate = 0.001f;
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;
    public float damage = 10.0f;
    public ParticleSystem[] muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    public Transform raycastOrigin;
    public AudioClip ShootingAudio;
    [Range(0, 1)] public float ShootingAudioVolume = 0.5f;
    public Vector3 testOffset;
    public LayerMask layerMask;


    public Transform raycastDestination;

    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;
    List<Bullet> bullets = new List<Bullet>();
    float maxLifetime = 3.0f;

    Vector3 GetPosition(Bullet bullet) {
      Vector3 gravity = Vector3.down * bulletDrop;
      return (bullet.initialPosition) + (bullet.initialVelocity *  bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity) {
      Bullet bullet = new Bullet();
      bullet.initialPosition = position;
      bullet.initialVelocity = velocity;
      bullet.time = 0.0f;
      bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
      bullet.tracer.AddPosition(position);
      return bullet;

    }

    public void StartFiring() {
        isFiring = true;
        accumulatedTime = 0.0f;
        // FireBullet(target);
    }

    public void UpdateWeapon(float deltaTime, Vector3 target) {
      if (isFiring) {
        UpdateFiring(deltaTime, target);
      }

      accumulatedTime += deltaTime;

      UpdateBullet(deltaTime);

    }

// Add to shooterAi
    public void UpdateFiring(float deltaTime, Vector3 target) {
      float fireInterval = 1.0f / fireRate;
      while(accumulatedTime >= 0.0f){
        FireBullet(target);
        accumulatedTime -= fireInterval;
      }
    }

    public void UpdateBullet(float deltaTime){
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    void SimulateBullets(float deltaTime){
        bullets.ForEach(bullet => {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0,p1,bullet);
          });
    }

    void DestroyBullets (){
      bullets.RemoveAll(bullet => bullet.time >= maxLifetime);
    }

    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet){
      Vector3 direction = end - start;
      float distance = direction.magnitude;
      ray.origin = start;
      ray.direction = direction;

      if(Physics.Raycast(ray,out hitInfo, distance, layerMask)) {
           Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 4.0f);
           hitEffect.transform.position = hitInfo.point;
           hitEffect.transform.forward = hitInfo.normal;
           hitEffect.Emit(1);

           var hitBox = hitInfo.collider.GetComponent<HitBox>();
           if (hitBox) {
             hitBox.OnRayCastHit(this, ray.direction);
           }

           bullet.tracer.transform.position = hitInfo.point;
           bullet.time = maxLifetime;
      } else {
        bullet.tracer.transform.position = end;
      }
    }

    private void FireBullet(Vector3 target) {
        foreach(var particle in muzzleFlash){
        particle.Emit(1);
        AudioSource.PlayClipAtPoint(ShootingAudio, ray.origin, ShootingAudioVolume);
        }

        Vector3 velocity = (target - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position , velocity);
        bullets.Add(bullet);

    }

    public void StopFiring() {
        isFiring = false;
    }

}
