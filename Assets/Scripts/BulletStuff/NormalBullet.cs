using UnityEngine;

public class NormalBullet : Bullet
{
    public override void Start()
    {
        base.Start();

        transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.TryGetComponent(out IDamageable damageable);
            other.TryGetComponent(out Enemy enemyObj);

            damageable.TakeDamage(bulletDamage);

            if (enemyObj.currentHealth <= 0)
                damageable.Despawn();

            //then destroys bullet
            Destroy(gameObject);
        }
    }
}
