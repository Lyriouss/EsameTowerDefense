using UnityEngine;

//Same as normal bullet but bullets isn't destroyed when hitting an enemy
public class SniperBullet : Bullet
{
    //Runs Start() of parent class
    public override void Start()
    {
        base.Start();

        //Additionally makes bullet a fixed size
        transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
    }

    //Runs FixedUpdate() of parent class
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        //When collided with an enemy
        if (other.CompareTag("Enemy"))
        {
            //Gets interface and script from enemy
            other.TryGetComponent(out IDamageable damageable);
            other.TryGetComponent(out Enemy enemyObj);

            //Deals damage to enemy
            damageable.TakeDamage(bulletDamage);

            //Checks if enemy health is equal to or below 0
            if (enemyObj.currentHealth <= 0)
                //If condition is true, despawns enemy
                damageable.Despawn();
        }
    }
}
