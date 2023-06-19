using UnityEngine;

public class Gun : RangedWeapon
{
    public Transform bulletSpawnPoint;
    public Transform bullet;
    public float bulletSpeed;

    private int bulletLayer;

    public void Awake() {
        bulletLayer = LayerMask.NameToLayer("Player Projectile");
    }

    protected override void Fire() {
        var bulletTrans = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bulletTrans.gameObject.layer = bulletLayer;
        var BulletRB = bulletTrans.GetComponent<Rigidbody>();
        BulletRB.AddRelativeForce(Vector3.forward * bulletSpeed);
        StartCoroutine(StartCooldown());
    }
}
